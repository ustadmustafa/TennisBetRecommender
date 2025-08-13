using System.Text.Json.Serialization;

namespace TennisBets.Models
{
    // API'den dönen response formatı
    public class PlayerStatsResponse
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }

        [JsonPropertyName("result")]
        public List<PlayerStats> Result { get; set; } = new();
    }

    public class PlayerStats
    {
        [JsonPropertyName("player_key")]
        public long PlayerKey { get; set; }

        [JsonPropertyName("player_name")]
        public string PlayerName { get; set; } = string.Empty;

        [JsonPropertyName("player_country")]
        public string PlayerCountry { get; set; } = string.Empty;

        [JsonPropertyName("stats")]
        public List<PlayerSeasonStats> Stats { get; set; } = new();
    }

    public class PlayerSeasonStats
    {
        [JsonPropertyName("season")]
        public string Season { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("rank")]
        public string Rank { get; set; } = string.Empty;

        [JsonPropertyName("titles")]
        public string Titles { get; set; } = string.Empty;

        [JsonPropertyName("matches_won")]
        public string MatchesWon { get; set; } = string.Empty;

        [JsonPropertyName("matches_lost")]
        public string MatchesLost { get; set; } = string.Empty;

        [JsonPropertyName("hard_won")]
        public string HardWon { get; set; } = string.Empty;

        [JsonPropertyName("hard_lost")]
        public string HardLost { get; set; } = string.Empty;

        [JsonPropertyName("clay_won")]
        public string ClayWon { get; set; } = string.Empty;

        [JsonPropertyName("clay_lost")]
        public string ClayLost { get; set; } = string.Empty;

        [JsonPropertyName("grass_won")]
        public string GrassWon { get; set; } = string.Empty;

        [JsonPropertyName("grass_lost")]
        public string GrassLost { get; set; } = string.Empty;
    }

    public class PlayerAnalysis
    {
        public long PlayerKey { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public OverallStats OverallStats { get; set; } = new();
        public SurfaceStats SurfaceStats { get; set; } = new();
        public RankingInfo RankingInfo { get; set; } = new();
    }

    public class OverallStats
    {
        public int TotalMatchesWon { get; set; }
        public int TotalMatchesLost { get; set; }
        public double WinRate { get; set; }
        public int TotalTitles { get; set; }
    }

    public class SurfaceStats
    {
        public SurfacePerformance Hard { get; set; } = new();
        public SurfacePerformance Clay { get; set; } = new();
        public SurfacePerformance Grass { get; set; } = new();
    }

    public class SurfacePerformance
    {
        public int Won { get; set; }
        public int Lost { get; set; }
        public double WinRate { get; set; }
    }

    public class RankingInfo
    {
        public string ATPRanking { get; set; } = "N/A";
        public string ATPPoints { get; set; } = "N/A";
        public string WTARanking { get; set; } = "N/A";
        public string WTAPoints { get; set; } = "N/A";
        public string CurrentLeague { get; set; } = "N/A";
    }

    public class MatchAnalysis
    {
        public PlayerAnalysis Player1 { get; set; } = new();
        public PlayerAnalysis Player2 { get; set; } = new();
        public string AnalysisDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
