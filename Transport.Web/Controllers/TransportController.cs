using Microsoft.AspNetCore.Mvc;
using Transport.Services;
using Transport.Services.DTOs;

namespace Transport.Web.Controllers
{
    public class TransportController : Controller
    {
        private readonly ITransportService service;
        public TransportController(ITransportService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            var stops = service.GetAllStops().ToList();
            ViewData["Stops"] = stops;
            ViewData["Routes"] = new List<RouteSearchResult>();
            ViewData["FromStop"] = "";
            ViewData["ToStop"] = "";
            return View();
        }

        public IActionResult Index(string fromStop, string toStop)
        {
            var stops = service.GetAllStops().ToList();
            var routes = new List<RouteSearchResult>();

            if (!string.IsNullOrEmpty(fromStop) && !string.IsNullOrEmpty(toStop))
            {
                routes = service.FindRoutes(fromStop, toStop).ToList();
            }

            ViewData["Stops"] = stops;
            ViewData["Routes"] = routes;
            ViewData["FromStop"] = fromStop;
            ViewData["ToStop"] = toStop;

            return View();
        }
    }
}
