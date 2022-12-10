using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models;
using AirTicketApp.Models.Flight;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace AirTicketApp.Controllers
{
    [Authorize]
    public class Flight : Controller
    {
        public int pageSize = 4;
        private readonly IFlightService flightService;
        private readonly IAirplaneService airplaneService;
        private readonly IAirportService airportService;
        private readonly ICompanyService companyService;

        public Flight(IFlightService _flightService,
            IAirplaneService _airplaneService,
            IAirportService _airportService,
            ICompanyService _companyService)
        {
            this.flightService = _flightService;            
            this.airplaneService = _airplaneService;            
            this.airportService = _airportService;            
            this.companyService = _companyService;            
        }

        /// <summary>
        /// Searching for flights
        /// </summary>
        /// <param name="page"></param>
        /// <returns>Filtered flight by giving date</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All(int page=1)
        {
            if (TempData["ShowAllFlights"] == "Yes")
            {
                IEnumerable<FlightViewModel> newQuery = new List<FlightViewModel>();
                try
                {
                    newQuery = await flightService.AllFlights();
                }
                catch (Exception)
                {

                    TempData[MessageConstant.ErrorMessage] = "System error!";
                    return View(newQuery.ToPagedList(page, pageSize));
                }
            }

            var filter = JsonConvert
                .DeserializeObject<List<FlightViewModel>>((string)TempData["filter"]);

            TempData["filter"] = JsonConvert.SerializeObject(filter);

            var pagedFlights = filter.ToPagedList(page, pageSize);

            return View(pagedFlights);
        }

        [HttpPost]        
        public async Task<IActionResult> Search(AllFlightsQueryModel query)
        {
            var result = await flightService.AllFlightsFilter(
                query.Sorting,
                query.SearchDate,
                query.ArrivalAirportId,
                query.DepartureAirportId                
                );
            //query.Flights = result.ToPagedList(query.page, pageSize);

            TempData["filter"] = JsonConvert.SerializeObject(result);
            return RedirectToAction("All", new { page = 1 });            
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var query = new AllFlightsQueryModel();

            query.Airports = await airportService.GetAllAirports();

            return View(query);
        }

        /// <summary>
        /// Detailed view in single fligt
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>return flight by given id</returns>
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
