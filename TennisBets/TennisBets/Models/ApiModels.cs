using System.Text.Json.Serialization;

namespace TennisBets.Models
{
    // H2H (Head-to-Head) API Response Models
    public class H2HResponse
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }

        [JsonPropertyName("result")]
        public H2HResult Result { get; set; } = new();
    }

    public class H2HResult
    {
        [JsonPropertyName("H2H")]
        public List<H2HMatch> H2H { get; set; } = new();

        [JsonPropertyName("firstPlayerResults")]
        public List<PlayerResult> FirstPlayerResults { get; set; } = new();

        [JsonPropertyName("secondPlayerResults")]
        public List<PlayerResult> SecondPlayerResults { get; set; } = new();
    }

    public class H2HMatch
    {
        [JsonPropertyName("event_key")]
        public long EventKey { get; set; }

        [JsonPropertyName("event_date")]
        public string EventDate { get; set; } = string.Empty;

        [JsonPropertyName("event_first_player")]
        public string EventFirstPlayer { get; set; } = string.Empty;

        [JsonPropertyName("event_second_player")]
        public string EventSecondPlayer { get; set; } = string.Empty;

        [JsonPropertyName("event_final_result")]
        public string EventFinalResult { get; set; } = string.Empty;

        [JsonPropertyName("event_winner")]
        public string EventWinner { get; set; } = string.Empty;

        [JsonPropertyName("event_status")]
        public string EventStatus { get; set; } = string.Empty;

        [JsonPropertyName("tournament_name")]
        public string TournamentName { get; set; } = string.Empty;

        [JsonPropertyName("tournament_round")]
        public string TournamentRound { get; set; } = string.Empty;
    }

    public class PlayerResult
    {
        [JsonPropertyName("event_key")]
        public long EventKey { get; set; }

        [JsonPropertyName("event_date")]
        public string EventDate { get; set; } = string.Empty;

        [JsonPropertyName("event_opponent")]
        public string EventOpponent { get; set; } = string.Empty;

        [JsonPropertyName("event_final_result")]
        public string EventFinalResult { get; set; } = string.Empty;

        [JsonPropertyName("event_winner")]
        public string EventWinner { get; set; } = string.Empty;

        [JsonPropertyName("event_status")]
        public string EventStatus { get; set; } = string.Empty;
    }

    // Standings API Response Models
    public class StandingsResponse
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }

        [JsonPropertyName("result")]
        public List<PlayerStanding> Result { get; set; } = new();
    }

    public class PlayerStanding
    {
        [JsonPropertyName("place")]
        public string Place { get; set; } = string.Empty;

        [JsonPropertyName("player")]
        public string Player { get; set; } = string.Empty;

        [JsonPropertyName("player_key")]
        public long? PlayerKey { get; set; }

        [JsonPropertyName("league")]
        public string League { get; set; } = string.Empty;

        [JsonPropertyName("movement")]
        public string Movement { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("points")]
        public string Points { get; set; } = string.Empty;
    }

    // Player Stats API Response Models
    public class PlayerResponse
    {
        [JsonPropertyName("success")]
        public int Success { get; set; }

        [JsonPropertyName("result")]
        public List<PlayerInfo> Result { get; set; } = new();
    }

    public class PlayerInfo
    {
        [JsonPropertyName("player_key")]
        public long PlayerKey { get; set; }

        [JsonPropertyName("player_name")]
        public string PlayerName { get; set; } = string.Empty;

        [JsonPropertyName("player_country")]
        public string PlayerCountry { get; set; } = string.Empty;

        [JsonPropertyName("stats")]
        public List<PlayerStat> Stats { get; set; } = new();
    }

    public class PlayerStat
    {
        [JsonPropertyName("season")]
        public string Season { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("rank")]
        public string Rank { get; set; } = string.Empty;

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
    }
}
