using AirTicketApp.Models;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AirTicketApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFlightService flightService;

        public HomeController(IFlightService _flightService)
        {
            this.flightService = _flightService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await flightService.GetMostCheapThreeFlights();

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}