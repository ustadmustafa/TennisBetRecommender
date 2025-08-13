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

                return new MatchAnalysis
                {
                    Player1 = player1Analysis,
                    Player2 = player2Analysis
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
