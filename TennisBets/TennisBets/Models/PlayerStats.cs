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
        [JsonPropertyName("playerKey")]
        public long PlayerKey { get; set; }
        
        [JsonPropertyName("playerName")]
        public string PlayerName { get; set; } = string.Empty;
        
        [JsonPropertyName("overallStats")]
        public OverallStats OverallStats { get; set; } = new();
        
        [JsonPropertyName("surfaceStats")]
        public SurfaceStats SurfaceStats { get; set; } = new();
        
        [JsonPropertyName("rankingInfo")]
        public RankingInfo RankingInfo { get; set; } = new();
    }

    public class OverallStats
    {
        [JsonPropertyName("totalMatchesWon")]
        public int TotalMatchesWon { get; set; }
        
        [JsonPropertyName("totalMatchesLost")]
        public int TotalMatchesLost { get; set; }
        
        [JsonPropertyName("winRate")]
        public double WinRate { get; set; }
        
        [JsonPropertyName("totalTitles")]
        public int TotalTitles { get; set; }
    }

    public class SurfaceStats
    {
        [JsonPropertyName("hard")]
        public SurfacePerformance Hard { get; set; } = new();
        
        [JsonPropertyName("clay")]
        public SurfacePerformance Clay { get; set; } = new();
        
        [JsonPropertyName("grass")]
        public SurfacePerformance Grass { get; set; } = new();
    }

    public class SurfacePerformance
    {
        [JsonPropertyName("won")]
        public int Won { get; set; }
        
        [JsonPropertyName("lost")]
        public int Lost { get; set; }
        
        [JsonPropertyName("winRate")]
        public double WinRate { get; set; }
    }

    public class RankingInfo
    {
        [JsonPropertyName("atpRanking")]
        public string ATPRanking { get; set; } = "N/A";
        
        [JsonPropertyName("atpPoints")]
        public string ATPPoints { get; set; } = "N/A";
        
        [JsonPropertyName("wtaRanking")]
        public string WTARanking { get; set; } = "N/A";
        
        [JsonPropertyName("wtaPoints")]
        public string WTAPoints { get; set; } = "N/A";
        
        [JsonPropertyName("currentLeague")]
        public string CurrentLeague { get; set; } = "N/A";
    }

    public class MatchAnalysis
    {
        [JsonPropertyName("player1")]
        public PlayerAnalysis Player1 { get; set; } = new();
        
        [JsonPropertyName("player2")]
        public PlayerAnalysis Player2 { get; set; } = new();
        
        [JsonPropertyName("h2hAnalysis")]
        public H2HAnalysisData H2HAnalysis { get; set; } = new();
        
        [JsonPropertyName("analysisDate")]
        public string AnalysisDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public class H2HAnalysisData
    {
        [JsonPropertyName("headToHeadMatches")]
        public List<H2HMatchData> HeadToHeadMatches { get; set; } = new();
        
        [JsonPropertyName("player1RecentMatches")]
        public List<PlayerRecentMatchData> Player1RecentMatches { get; set; } = new();
        
        [JsonPropertyName("player2RecentMatches")]
        public List<PlayerRecentMatchData> Player2RecentMatches { get; set; } = new();
    }

    public class H2HMatchData
    {
        [JsonPropertyName("eventDate")]
        public string EventDate { get; set; } = string.Empty;
        
        [JsonPropertyName("eventFinalResult")]
        public string EventFinalResult { get; set; } = string.Empty;
        
        [JsonPropertyName("winner")]
        public string Winner { get; set; } = string.Empty;
        
        [JsonPropertyName("tournamentName")]
        public string TournamentName { get; set; } = string.Empty;
        
        [JsonPropertyName("tournamentRound")]
        public string TournamentRound { get; set; } = string.Empty;
        
        [JsonPropertyName("eventTypeType")]
        public string EventTypeType { get; set; } = string.Empty;
    }

    public class PlayerRecentMatchData
    {
        [JsonPropertyName("eventDate")]
        public string EventDate { get; set; } = string.Empty;
        
        [JsonPropertyName("eventFirstPlayer")]
        public string EventFirstPlayer { get; set; } = string.Empty;
        
        [JsonPropertyName("eventSecondPlayer")]
        public string EventSecondPlayer { get; set; } = string.Empty;
        
        [JsonPropertyName("eventFinalResult")]
        public string EventFinalResult { get; set; } = string.Empty;
        
        [JsonPropertyName("winner")]
        public string Winner { get; set; } = string.Empty;
        
        [JsonPropertyName("tournamentName")]
        public string TournamentName { get; set; } = string.Empty;
        
        [JsonPropertyName("tournamentRound")]
        public string TournamentRound { get; set; } = string.Empty;
        
        [JsonPropertyName("eventTypeType")]
        public string EventTypeType { get; set; } = string.Empty;
    }
}
