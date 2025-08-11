using Microsoft.AspNetCore.Mvc;
using TennisBets.Models;
using TennisBets.Services;

namespace TennisBets.Controllers
{
    public class TennisController : Controller
    {
        private readonly ITennisService _tennisService;

        public TennisController(ITennisService tennisService)
        {
            _tennisService = tennisService;
        }

        public async Task<IActionResult> Index()
        {
            var liveMatches = await _tennisService.GetLiveMatchesAsync();
            var upcomingMatches = await _tennisService.GetUpcomingMatchesAsync();

            var viewModel = new TennisMatchesViewModel
            {
                LiveMatches = liveMatches,
                UpcomingMatches = upcomingMatches
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Live()
        {
            var matches = await _tennisService.GetLiveMatchesAsync();
            return View(matches);
        }

        public async Task<IActionResult> Upcoming()
        {
            var matches = await _tennisService.GetUpcomingMatchesAsync();
            return View(matches);
        }

        public async Task<IActionResult> Details(long id)
        {
            var match = await _tennisService.GetMatchDetailsAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }
    }

    public class TennisMatchesViewModel
    {
        public List<TennisMatch> LiveMatches { get; set; } = new();
        public List<TennisMatch> UpcomingMatches { get; set; } = new();
    }
}
