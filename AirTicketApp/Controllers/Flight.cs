using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirTicketApp.Controllers
{
    public class Flight : Controller
    {
        private readonly IFlightService flightService;

        public Flight(IFlightService flightService)
        {
            this.flightService = flightService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
                var query = await flightService.AllFlights();

            return View(query);
        }
    }
}
