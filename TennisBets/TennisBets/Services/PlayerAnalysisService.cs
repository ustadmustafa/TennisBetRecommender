using TennisBets.Models;

namespace TennisBets.Services
{
    public class PlayerAnalysisService : IPlayerAnalysisService
    {
        private readonly ITennisApiService _tennisApiService;

        public PlayerAnalysisService(ITennisApiService tennisApiService)
        {
            _tennisApiService = tennisApiService;
        }

        public async Task<MatchAnalysis> AnalyzeMatchAsync(long player1Key, long player2Key)
        {
            try
            {
                var player1Analysis = await AnalyzePlayerAsync(player1Key);
                var player2Analysis = await AnalyzePlayerAsync(player2Key);
                var h2hAnalysis = await GetH2HAnalysisAsync(player1Key, player2Key);

                return new MatchAnalysis
                {
                    Player1 = player1Analysis,
                    Player2 = player2Analysis,
                    H2HAnalysis = h2hAnalysis
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error analyzing match: {ex.Message}");
                throw;
            }
        }

        public async Task<PlayerAnalysis> AnalyzePlayerAsync(long playerKey)
        {
            try
            {
                var playerStats = await _tennisApiService.GetPlayerDetailedStatsAsync(playerKey);
                
                if (playerStats?.Stats == null || !playerStats.Stats.Any())
                {
                    return new PlayerAnalysis
                    {
                        PlayerKey = playerKey,
                        PlayerName = "Unknown Player"
                    };
                }

                var analysis = new PlayerAnalysis
                {
                    PlayerKey = playerKey,
                    PlayerName = playerStats.PlayerName,
                    OverallStats = CalculateOverallStats(playerStats.Stats),
                    SurfaceStats = CalculateSurfaceStats(playerStats.Stats),
                    RankingInfo = await GetPlayerRankingInfoAsync(playerKey)
                };

                return analysis;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error analyzing player {playerKey}: {ex.Message}");
                throw;
            }
        }

        private async Task<H2HAnalysisData> GetH2HAnalysisAsync(long player1Key, long player2Key)
        {
            try
            {
                Console.WriteLine($"Getting H2H analysis for players: {player1Key} vs {player2Key}");
                var h2hResponse = await _tennisApiService.GetH2HAsync(player1Key, player2Key);
                Console.WriteLine($"H2H API Response received: Success={h2hResponse?.Success}");
                Console.WriteLine($"H2H Response Result: H2H count={h2hResponse?.Result?.H2H?.Count ?? 0}, FirstPlayerResults count={h2hResponse?.Result?.FirstPlayerResults?.Count ?? 0}, SecondPlayerResults count={h2hResponse?.Result?.SecondPlayerResults?.Count ?? 0}");
                
                var h2hAnalysis = new H2HAnalysisData();

                // H2H maçlarını işle
                if (h2hResponse?.Result?.H2H != null)
                {
                    Console.WriteLine($"Processing {h2hResponse.Result.H2H.Count} H2H matches");
                    foreach (var match in h2hResponse.Result.H2H)
                    {
                        var h2hMatch = new H2HMatchData
                        {
                            EventDate = match.EventDate,
                            EventFinalResult = match.EventFinalResult,
                            Winner = GetWinnerName(match.EventWinner, match.EventFirstPlayer, match.EventSecondPlayer),
                            TournamentName = match.TournamentName,
                            TournamentRound = match.TournamentRound,
                            EventTypeType = match.EventTypeType
                        };
                        h2hAnalysis.HeadToHeadMatches.Add(h2hMatch);
                        Console.WriteLine($"Added H2H match: {match.EventDate} - {match.EventFirstPlayer} vs {match.EventSecondPlayer} - Winner: {h2hMatch.Winner}");
                    }
                }
                else
                {
                    Console.WriteLine("No H2H matches found in response");
                }

                // Player1'in son maçlarını işle (son 10 maç)
                if (h2hResponse?.Result?.FirstPlayerResults != null)
                {
                    Console.WriteLine($"Processing {h2hResponse.Result.FirstPlayerResults.Count} first player results");
                    var recentMatches = h2hResponse.Result.FirstPlayerResults.Take(10);
                    foreach (var match in recentMatches)
                    {
                        var recentMatch = new PlayerRecentMatchData
                        {
                            EventDate = match.EventDate,
                            EventFirstPlayer = match.EventFirstPlayer,
                            EventSecondPlayer = match.EventSecondPlayer,
                            EventFinalResult = match.EventFinalResult,
                            Winner = GetWinnerName(match.EventWinner, match.EventFirstPlayer, match.EventSecondPlayer),
                            TournamentName = match.TournamentName,
                            TournamentRound = match.TournamentRound,
                            EventTypeType = match.EventTypeType
                        };
                        h2hAnalysis.Player1RecentMatches.Add(recentMatch);
                        Console.WriteLine($"Added Player1 recent match: {match.EventDate} - {match.EventFirstPlayer} vs {match.EventSecondPlayer} - Winner: {recentMatch.Winner}");
                    }
                }
                else
                {
                    Console.WriteLine("No first player results found in response");
                }

                // Player2'nin son maçlarını işle (son 10 maç)
                if (h2hResponse?.Result?.SecondPlayerResults != null)
                {
                    Console.WriteLine($"Processing {h2hResponse.Result.SecondPlayerResults.Count} second player results");
                    var recentMatches = h2hResponse.Result.SecondPlayerResults.Take(10);
                    foreach (var match in recentMatches)
                    {
                        var recentMatch = new PlayerRecentMatchData
                        {
                            EventDate = match.EventDate,
                            EventFirstPlayer = match.EventFirstPlayer,
                            EventSecondPlayer = match.EventSecondPlayer,
                            EventFinalResult = match.EventFinalResult,
                            Winner = GetWinnerName(match.EventWinner, match.EventFirstPlayer, match.EventSecondPlayer),
                            TournamentName = match.TournamentName,
                            TournamentRound = match.TournamentRound,
                            EventTypeType = match.EventTypeType
                        };
                        h2hAnalysis.Player2RecentMatches.Add(recentMatch);
                        Console.WriteLine($"Added Player2 recent match: {match.EventDate} - {match.EventFirstPlayer} vs {match.EventSecondPlayer} - Winner: {recentMatch.Winner}");
                    }
                }
                else
                {
                    Console.WriteLine("No second player results found in response");
                }

                Console.WriteLine($"Final H2H Analysis: H2H={h2hAnalysis.HeadToHeadMatches.Count}, Player1Recent={h2hAnalysis.Player1RecentMatches.Count}, Player2Recent={h2hAnalysis.Player2RecentMatches.Count}");
                return h2hAnalysis;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting H2H analysis: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return new H2HAnalysisData();
            }
        }

        private string GetWinnerName(string eventWinner, string firstPlayer, string secondPlayer)
        {
            if (string.IsNullOrEmpty(eventWinner))
                return "Unknown";

            switch (eventWinner.ToLower())
            {
                case "first player":
                    return firstPlayer;
                case "second player":
                    return secondPlayer;
                default:
                    return eventWinner;
            }
        }

        private async Task<RankingInfo> GetPlayerRankingInfoAsync(long playerKey)
        {
            try
            {
                var rankingInfo = new RankingInfo();

                // ATP sıralamasını kontrol et
                var atpStandings = await _tennisApiService.GetStandingsAsync("ATP");
                var atpPlayer = atpStandings?.Result?.FirstOrDefault(p => p.PlayerKey.HasValue && p.PlayerKey.Value == playerKey);
                if (atpPlayer != null)
                {
                    rankingInfo.ATPRanking = atpPlayer.Place;
                    rankingInfo.ATPPoints = atpPlayer.Points;
                    rankingInfo.CurrentLeague = "ATP";
                }

                // WTA sıralamasını kontrol et
                var wtaStandings = await _tennisApiService.GetStandingsAsync("WTA");
                var wtaPlayer = wtaStandings?.Result?.FirstOrDefault(p => p.PlayerKey.HasValue && p.PlayerKey.Value == playerKey);
                if (wtaPlayer != null)
                {
                    rankingInfo.WTARanking = wtaPlayer.Place;
                    rankingInfo.WTAPoints = wtaPlayer.Points;
                    if (string.IsNullOrEmpty(rankingInfo.CurrentLeague))
                    {
                        rankingInfo.CurrentLeague = "WTA";
                    }
                }

                return rankingInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting ranking info for player {playerKey}: {ex.Message}");
                return new RankingInfo();
            }
        }

        private OverallStats CalculateOverallStats(List<PlayerSeasonStats> stats)
        {
            var overallStats = new OverallStats();
            
            foreach (var stat in stats)
            {
                if (int.TryParse(stat.MatchesWon, out int won))
                    overallStats.TotalMatchesWon += won;
                
                if (int.TryParse(stat.MatchesLost, out int lost))
                    overallStats.TotalMatchesLost += lost;
                
                if (int.TryParse(stat.Titles, out int titles))
                    overallStats.TotalTitles += titles;
            }

            var totalMatches = overallStats.TotalMatchesWon + overallStats.TotalMatchesLost;
            overallStats.WinRate = totalMatches > 0 ? (double)overallStats.TotalMatchesWon / totalMatches : 0;

            return overallStats;
        }

        private SurfaceStats CalculateSurfaceStats(List<PlayerSeasonStats> stats)
        {
            var surfaceStats = new SurfaceStats();

            foreach (var stat in stats)
            {
                // Hard surface stats
                if (int.TryParse(stat.HardWon, out int hardWon))
                    surfaceStats.Hard.Won += hardWon;
                if (int.TryParse(stat.HardLost, out int hardLost))
                    surfaceStats.Hard.Lost += hardLost;

                // Clay surface stats
                if (int.TryParse(stat.ClayWon, out int clayWon))
                    surfaceStats.Clay.Won += clayWon;
                if (int.TryParse(stat.ClayLost, out int clayLost))
                    surfaceStats.Clay.Lost += clayLost;

                // Grass surface stats
                if (int.TryParse(stat.GrassWon, out int grassWon))
                    surfaceStats.Grass.Won += grassWon;
                if (int.TryParse(stat.GrassLost, out int grassLost))
                    surfaceStats.Grass.Lost += grassLost;
            }

            // Calculate win rates for each surface
            var hardTotal = surfaceStats.Hard.Won + surfaceStats.Hard.Lost;
            surfaceStats.Hard.WinRate = hardTotal > 0 ? (double)surfaceStats.Hard.Won / hardTotal : 0;

            var clayTotal = surfaceStats.Clay.Won + surfaceStats.Clay.Lost;
            surfaceStats.Clay.WinRate = clayTotal > 0 ? (double)surfaceStats.Clay.Won / clayTotal : 0;

            var grassTotal = surfaceStats.Grass.Won + surfaceStats.Grass.Lost;
            surfaceStats.Grass.WinRate = grassTotal > 0 ? (double)surfaceStats.Grass.Won / grassTotal : 0;

            return surfaceStats;
        }
    }
}
