using TennisBets.Models;

namespace TennisBets.Services
{
    public interface ITennisService
    {
        Task<List<TennisMatch>> GetLiveMatchesAsync();
        Task<List<TennisMatch>> GetUpcomingMatchesAsync();
        Task<TennisMatch?> GetMatchDetailsAsync(long eventKey);
    }
}
