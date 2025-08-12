using Microsoft.AspNetCore.Mvc;
using TennisBets.Models;
using TennisBets.Services;

namespace TennisBets.Controllers
{
    public class TennisController : Controller
    {
        private readonly ITennisService _tennisService;
        private readonly IBettingPredictionService _bettingPredictionService;

        public TennisController(ITennisService tennisService, IBettingPredictionService bettingPredictionService)
        {
            _tennisService = tennisService;
            _bettingPredictionService = bettingPredictionService;
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

        // Bahis tahmin endpoint'i
        [HttpGet]
        public async Task<IActionResult> GetBettingPredictions(long player1Key, long player2Key)
        {
            try
            {
                Console.WriteLine($"GetBettingPredictions called with player1Key: {player1Key}, player2Key: {player2Key}");
                
                var predictions = await _bettingPredictionService.GetMatchPredictionsAsync(player1Key, player2Key);
                
                Console.WriteLine($"Predictions generated: {predictions?.Predictions?.Count ?? 0}");
                
                // Oyuncu isimleri ve maç ID'si JavaScript'ten gelecek
                // Burada sadece tahminleri döndür

                return Json(new { success = true, data = predictions });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBettingPredictions: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // H2H analiz endpoint'i
        [HttpGet]
        public async Task<IActionResult> GetH2HAnalysis(long player1Key, long player2Key)
        {
            try
            {
                var h2hAnalysis = await _bettingPredictionService.AnalyzeH2HAsync(player1Key, player2Key);
                return Json(new { success = true, data = h2hAnalysis });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Test endpoint'i - API'nin çalışıp çalışmadığını kontrol etmek için
        [HttpGet]
        public async Task<IActionResult> TestApi()
        {
            try
            {
                var h2hResponse = await _bettingPredictionService.AnalyzeH2HAsync(30, 67747); // Test oyuncuları
                return Json(new { success = true, message = "API test successful", data = h2hResponse });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }

    public class TennisMatchesViewModel
    {
        public List<TennisMatch> LiveMatches { get; set; } = new();
        public List<TennisMatch> UpcomingMatches { get; set; } = new();
    }
}
