using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirTicketApp.Controllers
{
    [Authorize]
    public class Flight : Controller
    {
        private readonly IFlightService flightService;

        public Flight(IFlightService flightService)
        {
            this.flightService = flightService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
                var query = await flightService.AllFlights();

            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new FlightViewModel()
            {
                Airplanes = await flightService.GetAllAirplanes(),
                Airports = await flightService.GetAllAirports(),
                Companies = await flightService.GetAllCompanies(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(FlightViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Airplanes = await flightService.GetAllAirplanes();
                model.Airports = await flightService.GetAllAirports();
                model.Companies = await flightService.GetAllCompanies();

                return View(model);
            }

            await flightService.Create(model);

            return RedirectToAction(nameof(All));
        }


    }
}
