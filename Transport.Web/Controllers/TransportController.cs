using Microsoft.AspNetCore.Mvc;
using Transport.Services;

namespace Transport.Web.Controllers
{
    public class TransportController : Controller
    {
        private readonly ITransportService _transportService;

        public TransportController(ITransportService transportService)
        {
            _transportService = transportService;
        }

        // Route-based: /Transport/Stops/{line}
        [Route("Transport/Stops/{line}")]
        public IActionResult Stops(string line)
        {
            var stops = _transportService.GetStopsForLine(line);
            ViewBag.Line = line;

            return View(stops);
        }
    }
}
