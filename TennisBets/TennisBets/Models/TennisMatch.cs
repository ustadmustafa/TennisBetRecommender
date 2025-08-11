using System.Text.Json.Serialization;

namespace TennisBets.Models
{
    public class TennisMatch
    {
        [JsonPropertyName("event_key")]
        public long EventKey { get; set; }

        [JsonPropertyName("event_date")]
        public string EventDate { get; set; } = string.Empty;

        [JsonPropertyName("event_time")]
        public string EventTime { get; set; } = string.Empty;

        [JsonPropertyName("event_first_player")]
        public string EventFirstPlayer { get; set; } = string.Empty;

        [JsonPropertyName("first_player_key")]
        public long FirstPlayerKey { get; set; }

        [JsonPropertyName("event_second_player")]
        public string EventSecondPlayer { get; set; } = string.Empty;

        [JsonPropertyName("second_player_key")]
        public long SecondPlayerKey { get; set; }

        [JsonPropertyName("event_final_result")]
        public string EventFinalResult { get; set; } = string.Empty;

        [JsonPropertyName("event_game_result")]
        public string EventGameResult { get; set; } = string.Empty;

        [JsonPropertyName("event_serve")]
        public string EventServe { get; set; } = string.Empty;

        [JsonPropertyName("event_winner")]
        public string? EventWinner { get; set; }

        [JsonPropertyName("event_status")]
        public string EventStatus { get; set; } = string.Empty;

        [JsonPropertyName("event_type_type")]
        public string EventTypeType { get; set; } = string.Empty;

        [JsonPropertyName("tournament_name")]
        public string TournamentName { get; set; } = string.Empty;

        [JsonPropertyName("tournament_key")]
        public long TournamentKey { get; set; }

        [JsonPropertyName("tournament_round")]
        public string TournamentRound { get; set; } = string.Empty;

        [JsonPropertyName("tournament_season")]
        public string TournamentSeason { get; set; } = string.Empty;

        [JsonPropertyName("event_live")]
        public string EventLive { get; set; } = string.Empty;

        [JsonPropertyName("event_first_player_logo")]
        public string? EventFirstPlayerLogo { get; set; }

        [JsonPropertyName("event_second_player_logo")]
        public string? EventSecondPlayerLogo { get; set; }

        [JsonPropertyName("event_qualification")]
        public string EventQualification { get; set; } = string.Empty;

        [JsonPropertyName("pointbypoint")]
        public List<PointByPoint> PointByPoint { get; set; } = new();

        [JsonPropertyName("scores")]
        public List<Score> Scores { get; set; } = new();

        [JsonPropertyName("statistics")]
        public List<Statistic> Statistics { get; set; } = new();

        public bool IsLive => EventLive == "1";
        public string MatchTime => $"{EventDate} {EventTime}";
        public string CurrentScore => EventGameResult;
        public string SetScore => string.Join(" - ", Scores.Select(s => $"{s.ScoreFirst}-{s.ScoreSecond}"));
    }

    public class PointByPoint
    {
        [JsonPropertyName("set_number")]
        public string SetNumber { get; set; } = string.Empty;

        [JsonPropertyName("number_game")]
        public string NumberGame { get; set; } = string.Empty;

        [JsonPropertyName("player_served")]
        public string PlayerServed { get; set; } = string.Empty;

        [JsonPropertyName("serve_winner")]
        public string? ServeWinner { get; set; }

        [JsonPropertyName("serve_lost")]
        public string? ServeLost { get; set; }

        [JsonPropertyName("score")]
        public string Score { get; set; } = string.Empty;

        [JsonPropertyName("points")]
        public List<Point> Points { get; set; } = new();
    }

    public class Point
    {
        [JsonPropertyName("number_point")]
        public string NumberPoint { get; set; } = string.Empty;

        [JsonPropertyName("score")]
        public string Score { get; set; } = string.Empty;

        [JsonPropertyName("break_point")]
        public string? BreakPoint { get; set; }

        [JsonPropertyName("set_point")]
        public string? SetPoint { get; set; }

        [JsonPropertyName("match_point")]
        public string? MatchPoint { get; set; }
    }

    public class Score
    {
        [JsonPropertyName("score_first")]
        public string ScoreFirst { get; set; } = string.Empty;

        [JsonPropertyName("score_second")]
        public string ScoreSecond { get; set; } = string.Empty;

        [JsonPropertyName("score_set")]
        public string ScoreSet { get; set; } = string.Empty;
    }

    public class Statistic
    {
        [JsonPropertyName("player_key")]
        public long PlayerKey { get; set; }

        [JsonPropertyName("stat_period")]
        public string StatPeriod { get; set; } = string.Empty;

        [JsonPropertyName("stat_type")]
        public string StatType { get; set; } = string.Empty;

        [JsonPropertyName("stat_name")]
        public string StatName { get; set; } = string.Empty;

        [JsonPropertyName("stat_value")]
        public string StatValue { get; set; } = string.Empty;

        [JsonPropertyName("stat_won")]
        public int? StatWon { get; set; }

        [JsonPropertyName("stat_total")]
        public int? StatTotal { get; set; }
    }
}
