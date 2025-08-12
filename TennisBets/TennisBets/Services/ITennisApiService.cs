using TennisBets.Models;

namespace TennisBets.Services
{
    public interface ITennisApiService
    {
        Task<H2HResponse> GetH2HAsync(long player1Key, long player2Key);
        Task<StandingsResponse> GetStandingsAsync(string eventType = "WTA");
        Task<PlayerResponse> GetPlayerStatsAsync(long playerKey);
        Task<PlayerStats> GetPlayerDetailedStatsAsync(long playerKey);
    }
}
