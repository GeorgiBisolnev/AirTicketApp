using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Models;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirTicketApp.Areas.Admin.Controllers
{
    [Authorize]
    public class Flight : BaseController
    {
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

        [HttpGet]
        public async Task<IActionResult> Add()
        {            
            var model = new FlightViewModel()
            {                
                Airplanes = await airplaneService.GetAllAirplanes(),
                Airports = await airportService.GetAllAirports(),
                Companies = await companyService.GetAllCompanies(),
                DateTimeNowFormated = DateTime.Now.ToString("yyyy-MM-dd") + 
                "T"+
                DateTime.Now.ToString("HH:mm"),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(FlightViewModel model)
        {

            if (!ModelState.IsValid)
            {
                try
                {
                    model.Airplanes = await airplaneService.GetAllAirplanes();
                    model.Airports = await airportService.GetAllAirports();
                    model.Companies = await companyService.GetAllCompanies();
                }
                catch (Exception)
                {

                    TempData[MessageConstant.ErrorMessage] = "System error!";
                    return View(model);
                }
                
                if (string.IsNullOrEmpty(model.DateTimeNowFormated))
                {
                    model.DateTimeNowFormated = DateTime.Now.ToString("yyyy-MM-dd") +
                    "T" +
                    DateTime.Now.ToString("HH:mm");
                }
                TempData[MessageConstant.WarningMessage] = "Not all validations are passed!";
                return View(model);
            }

            if (model.DepartureDate<DateTime.Now)
            {
                TempData[MessageConstant.WarningMessage] = "Departure date error!";
                return View(model);
            }

            try
            {
                await flightService.Create(model);
            }
            catch (Exception)
            {
                TempData[MessageConstant.ErrorMessage] = "Can not create new flight! System error!";
                return View(model);
            }

            TempData[MessageConstant.SuccessMessage] = "Successfully added new flight!";
            return RedirectToAction("All", "Flight", new { area = "" });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {

            FlightViewModel flightModel = new FlightViewModel();
            try
            {
                flightModel = await flightService.Details(Id);
            }
            catch (Exception)
            {

                TempData[MessageConstant.ErrorMessage] = "Cant load flight details due to system error!";
            }
            
            return View(flightModel);
        }


    }
}
