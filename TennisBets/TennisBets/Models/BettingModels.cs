namespace TennisBets.Models
{
    // Bahis Tahmin Sonucu
    public class BettingPrediction
    {
        public string BetType { get; set; } = string.Empty;
        public string Prediction { get; set; } = string.Empty;
        public double Confidence { get; set; } // 0.0 - 1.0 arası
        public string Reasoning { get; set; } = string.Empty;
        public List<string> DataSources { get; set; } = new();
        public double RecommendedOdds { get; set; }
        public BettingRecommendation Recommendation { get; set; }
    }

    // Bahis Önerisi
    public enum BettingRecommendation
    {
        StrongBet,      // Güçlü bahis önerisi
        ModerateBet,    // Orta seviye bahis önerisi
        WeakBet,        // Zayıf bahis önerisi
        AvoidBet        // Bahis yapmaktan kaçın
    }

    // Maç için Tüm Bahis Tahminleri
    public class MatchBettingPredictions
    {
        public long MatchId { get; set; }
        public string Player1Name { get; set; } = string.Empty;
        public string Player2Name { get; set; } = string.Empty;
        public List<BettingPrediction> Predictions { get; set; } = new();
        public DateTime PredictionDate { get; set; } = DateTime.Now;
        public double OverallConfidence { get; set; }
    }

    // H2H Analiz Sonucu
    public class H2HAnalysis
    {
        public int TotalMatches { get; set; }
        public int Player1Wins { get; set; }
        public int Player2Wins { get; set; }
        public double Player1WinRate { get; set; }
        public double Player2WinRate { get; set; }
        public List<string> RecentResults { get; set; } = new();
        public double AverageSets { get; set; }
        public double FirstSetPlayer1WinRate { get; set; }
        public double FirstSetPlayer2WinRate { get; set; }
        public int TieBreakCount { get; set; }
        public double TieBreakRate { get; set; }
    }

    // Oyuncu Performans Analizi
    public class PlayerPerformanceAnalysis
    {
        public long PlayerKey { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int? Ranking { get; set; }
        public int? Points { get; set; }
        public double SeasonWinRate { get; set; }
        public int TotalMatches { get; set; }
        public double HardCourtWinRate { get; set; }
        public double ClayCourtWinRate { get; set; }
        public string League { get; set; } = string.Empty;
    }
}
