using TennisBets.Models;

namespace TennisBets.Services
{
    public interface IBettingPredictionService
    {
        Task<MatchBettingPredictions> GetMatchPredictionsAsync(long player1Key, long player2Key);
        Task<H2HAnalysis> AnalyzeH2HAsync(long player1Key, long player2Key);
        Task<PlayerPerformanceAnalysis> AnalyzePlayerPerformanceAsync(long playerKey);
        Task<List<BettingPrediction>> GenerateAllBettingPredictionsAsync(long player1Key, long player2Key);
    }
}
