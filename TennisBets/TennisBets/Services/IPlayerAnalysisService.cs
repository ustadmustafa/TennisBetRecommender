using TennisBets.Models;

namespace TennisBets.Services
{
    public interface IPlayerAnalysisService
    {
        Task<MatchAnalysis> AnalyzeMatchAsync(long player1Key, long player2Key);
        Task<PlayerAnalysis> AnalyzePlayerAsync(long playerKey);
    }
}
