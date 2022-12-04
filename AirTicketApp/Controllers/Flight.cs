using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace AirTicketApp.Controllers
{
    [Authorize]
    public class Flight : Controller
    {
        public int pageSize = 4;
        private readonly IFlightService flightService;

        public Flight(IFlightService flightService)
        {
            this.flightService = flightService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All(int page=1)
        {
            IEnumerable<FlightViewModel> query = new List<FlightViewModel>();
            try
            {
                query = await flightService.AllFlights();
            }
            catch (Exception)
            {

                TempData[MessageConstant.ErrorMessage] = "System error!";
                return View(query.ToPagedList(page, pageSize));
            }

            //Filter all flights

            var pagedFlights = query.ToPagedList(page, pageSize);

            return View(pagedFlights);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            FlightViewModelDetails flightModel = new FlightViewModelDetails();
            try
            {
                flightModel = await flightService.Details(Id);
            }
            catch (Exception)
            {

                TempData[MessageConstant.ErrorMessage] = "System error!";
                return RedirectToAction("All");
            }            

            return View(flightModel);
        }


    }
}
