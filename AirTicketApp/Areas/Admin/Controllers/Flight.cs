using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models.FlightModels;
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
                DateTimeNowFormatedArrival = DateTime.Now.ToString("yyyy-MM-dd") + 
                "T"+
                DateTime.Now.ToString("HH:mm"),
                DateTimeNowFormatedDeparture = DateTime.Now.ToString("yyyy-MM-dd") +
                "T" +
                DateTime.Now.ToString("HH:mm"),                
            };

            model.ArrivalAirport = new Airport()
            {
                Id = 1
            };
            model.DepartureAirport = new Airport()
            {
                Id = 1
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(FlightViewModel model)
        {
            if (model.ArrivalAirport == null)
            {
                model.ArrivalAirport = new Airport()
                {
                    Id = 0
                };
            }
            if (model.DepartureAirport == null)
            {
                model.DepartureAirport = new Airport()
                {
                    Id = 0
                };
            }
            if (!ModelState.IsValid || model.DepartureDate < DateTime.Now)
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
                
                model.DateTimeNowFormatedArrival = model.ArrivalDate.ToString("yyyy-MM-dd") +
                    "T" + model.ArrivalDate.ToString("HH:mm");

                model.DateTimeNowFormatedDeparture = model.DepartureDate.ToString("yyyy-MM-dd") +
                    "T" + model.DepartureDate.ToString("HH:mm");
                TempData[MessageConstant.ErrorMessage] = "Departure date error!";
                TempData[MessageConstant.WarningMessage] = "Not all validations are passed!";
                return View(model);
            }

            int flightCreatedId = 0;
            try
            {
                flightCreatedId = await flightService.Create(model);
            }
            catch (Exception)
            {
                TempData[MessageConstant.ErrorMessage] = "Can not create new flight! System error!";
                return View(model);
            }

            TempData[MessageConstant.SuccessMessage] = "Successfully added new flight!";
            return RedirectToAction("Details","Flight", new { Id=flightCreatedId, Area = "" });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var model = await flightService.GetFlightById(Id);
            model.Airplanes = await airplaneService.GetAllAirplanes();
            model.Airports = await airportService.GetAllAirports();
            model.Companies = await companyService.GetAllCompanies();
            model.DateTimeNowFormatedArrival = model.ArrivalDate.ToString("yyyy-MM-dd") +
            "T" + model.ArrivalDate.ToString("HH:mm");
            model.DateTimeNowFormatedDeparture = model.DepartureDate.ToString("yyyy-MM-dd") +
            "T" + model.DepartureDate.ToString("HH:mm");


            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, FlightViewModel model)
        {
            if (model.ArrivalAirport == null)
            {
                model.ArrivalAirport = new Airport()
                {
                    Id = 0
                };
            }
            if (model.DepartureAirport == null)
            {
                model.DepartureAirport = new Airport()
                {
                    Id = 0
                };
            }
            if (!ModelState.IsValid || model.DepartureDate < DateTime.Now)
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

                model.DateTimeNowFormatedArrival = model.ArrivalDate.ToString("yyyy-MM-dd") +
                    "T" + model.ArrivalDate.ToString("HH:mm");

                model.DateTimeNowFormatedDeparture = model.DepartureDate.ToString("yyyy-MM-dd") +
                    "T" + model.DepartureDate.ToString("HH:mm");
                TempData[MessageConstant.ErrorMessage] = "Departure date error!";
                TempData[MessageConstant.WarningMessage] = "Not all validations are passed!";
                return View(model);
            }

            try
            {
                await flightService.Edit(id, model);
            }
            catch (ArgumentException ex)
            {
                TempData[MessageConstant.ErrorMessage] = "Can not edit flight! " + ex;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = "Can not edit flight! System error! " + ex;
                return View(model);
            }

            TempData[MessageConstant.SuccessMessage] = "Successfully edited flight!";
            return RedirectToAction("Details", "Flight", new { Id = id, Area = "" });
        }
    }
}
