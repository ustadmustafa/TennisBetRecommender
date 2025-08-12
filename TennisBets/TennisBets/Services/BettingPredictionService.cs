using TennisBets.Models;

namespace TennisBets.Services
{
    public class BettingPredictionService : IBettingPredictionService
    {
        private readonly ITennisApiService _tennisApiService;

        public BettingPredictionService(ITennisApiService tennisApiService)
        {
            _tennisApiService = tennisApiService;
        }

        public async Task<MatchBettingPredictions> GetMatchPredictionsAsync(long player1Key, long player2Key)
        {
            try
            {
                Console.WriteLine($"GetMatchPredictionsAsync called with player1Key: {player1Key}, player2Key: {player2Key}");
                
                var predictions = await GenerateAllBettingPredictionsAsync(player1Key, player2Key);
                
                Console.WriteLine($"Generated {predictions.Count} predictions");
                
                var overallConfidence = predictions.Any() ? predictions.Average(p => p.Confidence) : 0.0;

                var result = new MatchBettingPredictions
                {
                    MatchId = 0, // Bu değer controller'dan gelecek
                    Player1Name = "", // Bu değer controller'dan gelecek
                    Player2Name = "", // Bu değer controller'dan gelecek
                    Predictions = predictions,
                    OverallConfidence = overallConfidence
                };
                
                Console.WriteLine($"Returning MatchBettingPredictions with {result.Predictions.Count} predictions");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMatchPredictionsAsync: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<H2HAnalysis> AnalyzeH2HAsync(long player1Key, long player2Key)
        {
            var h2hData = await _tennisApiService.GetH2HAsync(player1Key, player2Key);
            
            if (h2hData.Result?.H2H == null || !h2hData.Result.H2H.Any())
            {
                return new H2HAnalysis();
            }

            var h2hMatches = h2hData.Result.H2H.Where(m => m.EventStatus == "Finished").ToList();
            
            if (!h2hMatches.Any())
            {
                return new H2HAnalysis();
            }

            var player1Wins = h2hMatches.Count(m => m.EventWinner == "First Player");
            var player2Wins = h2hMatches.Count(m => m.EventWinner == "Second Player");
            var totalMatches = h2hMatches.Count;

            var analysis = new H2HAnalysis
            {
                TotalMatches = totalMatches,
                Player1Wins = player1Wins,
                Player2Wins = player2Wins,
                Player1WinRate = totalMatches > 0 ? (double)player1Wins / totalMatches : 0,
                Player2WinRate = totalMatches > 0 ? (double)player2Wins / totalMatches : 0,
                RecentResults = h2hMatches.TakeLast(5).Select(m => $"{m.EventFirstPlayer} vs {m.EventSecondPlayer}: {m.EventFinalResult}").ToList()
            };

            // Set analizi (basit hesaplama - gerçek set skorları API'de yok)
            analysis.AverageSets = 2.5; // Varsayılan değer
            analysis.FirstSetPlayer1WinRate = 0.5; // Varsayılan değer
            analysis.FirstSetPlayer2WinRate = 0.5; // Varsayılan değer
            analysis.TieBreakCount = 0; // API'de tie-break bilgisi yok
            analysis.TieBreakRate = 0;

            return analysis;
        }

        public async Task<PlayerPerformanceAnalysis> AnalyzePlayerPerformanceAsync(long playerKey)
        {
            var playerData = await _tennisApiService.GetPlayerStatsAsync(playerKey);
            var standingsData = await _tennisApiService.GetStandingsAsync();

            var playerInfo = playerData.Result?.FirstOrDefault();
            var standing = standingsData.Result?.FirstOrDefault(s => s.PlayerKey == playerKey);

            if (playerInfo == null)
            {
                return new PlayerPerformanceAnalysis { PlayerKey = playerKey };
            }

            var currentSeasonStats = playerInfo.Stats?.FirstOrDefault(s => s.Season == DateTime.Now.Year.ToString()) 
                                   ?? playerInfo.Stats?.FirstOrDefault();

            var analysis = new PlayerPerformanceAnalysis
            {
                PlayerKey = playerKey,
                PlayerName = playerInfo.PlayerName,
                Ranking = standing != null ? int.TryParse(standing.Place, out var rank) ? rank : null : null,
                Points = standing != null ? int.TryParse(standing.Points, out var points) ? points : null : null,
                League = standing?.League ?? "",
                TotalMatches = 0,
                SeasonWinRate = 0,
                HardCourtWinRate = 0,
                ClayCourtWinRate = 0
            };

            if (currentSeasonStats != null)
            {
                var matchesWon = int.TryParse(currentSeasonStats.MatchesWon, out var won) ? won : 0;
                var matchesLost = int.TryParse(currentSeasonStats.MatchesLost, out var lost) ? lost : 0;
                var totalMatches = won + lost;

                analysis.TotalMatches = totalMatches;
                analysis.SeasonWinRate = totalMatches > 0 ? (double)won / totalMatches : 0;

                // Zemin bazlı performans
                var hardWon = int.TryParse(currentSeasonStats.HardWon, out var hw) ? hw : 0;
                var hardLost = int.TryParse(currentSeasonStats.HardLost, out var hl) ? hl : 0;
                var hardTotal = hardWon + hardLost;
                analysis.HardCourtWinRate = hardTotal > 0 ? (double)hardWon / hardTotal : 0;

                var clayWon = int.TryParse(currentSeasonStats.ClayWon, out var cw) ? cw : 0;
                var clayLost = int.TryParse(currentSeasonStats.ClayLost, out var cl) ? cl : 0;
                var clayTotal = clayWon + clayLost;
                analysis.ClayCourtWinRate = clayTotal > 0 ? (double)clayWon / clayTotal : 0;
            }

            return analysis;
        }

        public async Task<List<BettingPrediction>> GenerateAllBettingPredictionsAsync(long player1Key, long player2Key)
        {
            var predictions = new List<BettingPrediction>();

            // H2H ve oyuncu performans verilerini al
            var h2hAnalysis = await AnalyzeH2HAsync(player1Key, player2Key);
            var player1Analysis = await AnalyzePlayerPerformanceAsync(player1Key);
            var player2Analysis = await AnalyzePlayerPerformanceAsync(player2Key);

            // 1. Maç Kazananı Bahsi
            predictions.Add(GenerateMatchWinnerPrediction(h2hAnalysis, player1Analysis, player2Analysis));

            // 2. Toplam Set Bahsi
            predictions.Add(GenerateTotalSetsPrediction(h2hAnalysis));

            // 3. İlk Set Kazananı Bahsi
            predictions.Add(GenerateFirstSetWinnerPrediction(h2hAnalysis, player1Analysis, player2Analysis));

            // 4. Handikap Bahsi
            predictions.Add(GenerateHandicapPrediction(h2hAnalysis, player1Analysis, player2Analysis));

            // 5. Comeback Bahsi
            predictions.Add(GenerateComebackPrediction(h2hAnalysis));

            return predictions;
        }

        private BettingPrediction GenerateMatchWinnerPrediction(H2HAnalysis h2h, PlayerPerformanceAnalysis player1, PlayerPerformanceAnalysis player2)
        {
            // H2H verisi var mı kontrol et
            var hasH2HData = h2h.TotalMatches > 0;
            var hasRankingData = player1.Ranking.HasValue && player2.Ranking.HasValue;
            
            if (!hasH2HData && !hasRankingData)
            {
                // Hiç veri yok - varsayılan tahmin
                return new BettingPrediction
                {
                    BetType = "Maç Kazananı",
                    Prediction = "Yetersiz veri",
                    Confidence = 0.0,
                    Reasoning = "H2H geçmişi ve sıralama bilgisi bulunamadı. Bu maç için yeterli veri yok.",
                    DataSources = new List<string> { "Veri bulunamadı" },
                    RecommendedOdds = 1.5,
                    Recommendation = BettingRecommendation.AvoidBet
                };
            }

            // H2H geçmiş galibiyet oranları + oyuncu sıralaması
            var h2hWeight = hasH2HData ? 0.6 : 0.0; // H2H ağırlığı
            var rankingWeight = hasRankingData ? 0.4 : 0.0; // Sıralama ağırlığı

            var h2hConfidence = hasH2HData ? Math.Abs(h2h.Player1WinRate - h2h.Player2WinRate) : 0.0;
            
            // Sıralama bazlı güven
            var rankingConfidence = 0.0;
            if (hasRankingData)
            {
                var rankingDiff = Math.Abs(player1.Ranking.Value - player2.Ranking.Value);
                rankingConfidence = rankingDiff > 100 ? 0.8 : rankingDiff > 50 ? 0.6 : rankingDiff > 20 ? 0.4 : 0.2;
            }

            var totalConfidence = (h2hConfidence * h2hWeight) + (rankingConfidence * rankingWeight);
            
            // Tahmin yap
            string prediction;
            if (hasH2HData && hasRankingData)
            {
                // Her iki veri de var
                prediction = h2h.Player1WinRate > h2h.Player2WinRate ? 
                    (player1.PlayerName ?? "Player 1") : (player2.PlayerName ?? "Player 2");
            }
            else if (hasH2HData)
            {
                // Sadece H2H var
                prediction = h2h.Player1WinRate > h2h.Player2WinRate ? 
                    (player1.PlayerName ?? "Player 1") : (player2.PlayerName ?? "Player 2");
            }
            else
            {
                // Sadece sıralama var
                prediction = (player1.Ranking ?? 999) < (player2.Ranking ?? 999) ? 
                    (player1.PlayerName ?? "Player 1") : (player2.PlayerName ?? "Player 2");
            }
            
            // Sıfıra bölme kontrolü
            var recommendedOdds = 1.5; // Varsayılan değer
            if (hasH2HData && h2h.Player1WinRate > h2h.Player2WinRate)
            {
                if (h2h.Player1WinRate > 0)
                    recommendedOdds = Math.Max(1.0 / h2h.Player1WinRate, 1.1);
            }
            else if (hasH2HData)
            {
                if (h2h.Player2WinRate > 0)
                    recommendedOdds = Math.Max(1.0 / h2h.Player2WinRate, 1.1);
            }

            // Reasoning mesajını iyileştir
            var reasoning = "";
            if (hasH2HData)
                reasoning += $"H2H: {h2h.Player1WinRate:P0} vs {h2h.Player2WinRate:P0}";
            
            if (hasRankingData)
            {
                if (reasoning.Length > 0) reasoning += ", ";
                reasoning += $"Sıralama: {player1.Ranking} vs {player2.Ranking}";
            }
            
            if (reasoning.Length == 0)
                reasoning = "Sınırlı veri";

            return new BettingPrediction
            {
                BetType = "Maç Kazananı",
                Prediction = prediction,
                Confidence = Math.Min(totalConfidence, 0.95),
                Reasoning = reasoning,
                DataSources = new List<string> { 
                    hasH2HData ? "H2H Geçmiş" : "H2H Yok",
                    hasRankingData ? "Oyuncu Sıralaması" : "Sıralama Yok"
                },
                RecommendedOdds = Math.Max(recommendedOdds, 1.1),
                Recommendation = GetRecommendation(totalConfidence)
            };
        }

        private BettingPrediction GenerateTotalSetsPrediction(H2HAnalysis h2h)
        {
            // H2H verisi var mı kontrol et
            if (h2h.TotalMatches == 0)
            {
                return new BettingPrediction
                {
                    BetType = "Toplam Set",
                    Prediction = "Yetersiz veri",
                    Confidence = 0.0,
                    Reasoning = "H2H geçmişi bulunamadı. Set analizi yapılamıyor.",
                    DataSources = new List<string> { "H2H Yok" },
                    RecommendedOdds = 1.8,
                    Recommendation = BettingRecommendation.AvoidBet
                };
            }

            // H2H maçların ortalama set sayısı (varsayılan değerler kullanılıyor)
            var avgSets = h2h.AverageSets;
            var prediction = avgSets > 2.5 ? "3+ Set" : "2 Set";
            var confidence = 0.4; // Düşük güven çünkü set skorları API'de yok

            return new BettingPrediction
            {
                BetType = "Toplam Set",
                Prediction = prediction,
                Confidence = confidence,
                Reasoning = $"Ortalama set sayısı: {avgSets:F1} (H2H: {h2h.TotalMatches} maç)",
                DataSources = new List<string> { "H2H Maç Geçmişi" },
                RecommendedOdds = 1.8,
                Recommendation = BettingRecommendation.WeakBet
            };
        }

        private BettingPrediction GenerateFirstSetWinnerPrediction(H2HAnalysis h2h, PlayerPerformanceAnalysis player1, PlayerPerformanceAnalysis player2)
        {
            // H2H ilk set sonuçları + oyuncu sezon performansı
            var h2hWeight = 0.7;
            var performanceWeight = 0.3;

            var h2hConfidence = Math.Abs(h2h.FirstSetPlayer1WinRate - h2h.FirstSetPlayer2WinRate);
            var performanceConfidence = Math.Abs(player1.SeasonWinRate - player2.SeasonWinRate);

            var totalConfidence = (h2hConfidence * h2hWeight) + (performanceConfidence * performanceWeight);
            var prediction = h2h.FirstSetPlayer1WinRate > h2h.FirstSetPlayer2WinRate ? 
                (player1.PlayerName ?? "Player 1") : (player2.PlayerName ?? "Player 2");

            return new BettingPrediction
            {
                BetType = "İlk Set Kazananı",
                Prediction = prediction,
                Confidence = Math.Min(totalConfidence, 0.85),
                Reasoning = $"İlk set H2H: {h2h.FirstSetPlayer1WinRate:P0} vs {h2h.FirstSetPlayer2WinRate:P0}",
                DataSources = new List<string> { "H2H İlk Set", "Sezon Performansı" },
                RecommendedOdds = 1.9,
                Recommendation = GetRecommendation(totalConfidence)
            };
        }

        private BettingPrediction GenerateHandicapPrediction(H2HAnalysis h2h, PlayerPerformanceAnalysis player1, PlayerPerformanceAnalysis player2)
        {
            // H2H maç skor farkları + oyuncu sezon performansı
            var h2hConfidence = Math.Abs(h2h.Player1WinRate - h2h.Player2WinRate);
            var performanceDiff = Math.Abs(player1.SeasonWinRate - player2.SeasonWinRate);
            
            var totalConfidence = (h2hConfidence * 0.6) + (performanceDiff * 0.4);
            
            var prediction = h2h.Player1WinRate > h2h.Player2WinRate ? 
                $"{(player1.PlayerName ?? "Player 1")} (-1.5)" : $"{(player2.PlayerName ?? "Player 2")} (-1.5)";

            return new BettingPrediction
            {
                BetType = "Handikap (-1.5)",
                Prediction = prediction,
                Confidence = Math.Min(totalConfidence, 0.75),
                Reasoning = $"H2H fark: {Math.Abs(h2h.Player1WinRate - h2h.Player2WinRate):P0}, Performans fark: {performanceDiff:P0}",
                DataSources = new List<string> { "H2H Maç Skorları", "Sezon Performansı" },
                RecommendedOdds = 1.85,
                Recommendation = GetRecommendation(totalConfidence)
            };
        }

        private BettingPrediction GenerateComebackPrediction(H2HAnalysis h2h)
        {
            // H2H geçmiş maçlarda ilk seti kaybeden oyuncunun maç kazanma oranı
            // Bu veri API'de mevcut değil, varsayılan değerler kullanılıyor
            var confidence = 0.3; // Düşük güven
            var prediction = "İlk seti kaybeden oyuncu maçı kazanır";

            return new BettingPrediction
            {
                BetType = "Comeback Bahsi",
                Prediction = prediction,
                Confidence = confidence,
                Reasoning = "Yetersiz veri - ilk set skorları API'de mevcut değil",
                DataSources = new List<string> { "H2H Maç Geçmişi (Sınırlı)" },
                RecommendedOdds = 3.5,
                Recommendation = BettingRecommendation.AvoidBet
            };
        }

        private BettingRecommendation GetRecommendation(double confidence)
        {
            return confidence switch
            {
                >= 0.8 => BettingRecommendation.StrongBet,
                >= 0.6 => BettingRecommendation.ModerateBet,
                >= 0.4 => BettingRecommendation.WeakBet,
                _ => BettingRecommendation.AvoidBet
            };
        }
    }
}
