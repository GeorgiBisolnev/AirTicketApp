﻿using AirTicketApp.Data.Common.MessageConstants;
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

        //[HttpGet]
        //public async Task<IActionResult> Add()
        //{
        //    var model = new FlightViewModel()
        //    {                
        //        Airplanes = await flightService.GetAllAirplanes(),
        //        Airports = await flightService.GetAllAirports(),
        //        Companies = await flightService.GetAllCompanies(),
        //        DateTimeNowFormated = DateTime.Now.ToString("yyyy-MM-dd") + 
        //        "T"+
        //        DateTime.Now.ToString("HH:mm"),
        //    };

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(FlightViewModel model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        model.Airplanes = await flightService.GetAllAirplanes();
        //        model.Airports = await flightService.GetAllAirports();
        //        model.Companies = await flightService.GetAllCompanies();
        //        if (string.IsNullOrEmpty(model.DateTimeNowFormated))
        //        {
        //            model.DateTimeNowFormated = DateTime.Now.ToString("yyyy-MM-dd") +
        //            "T" +
        //            DateTime.Now.ToString("HH:mm");
        //        }
        //        return View(model);
        //    }

        //    await flightService.Create(model);

        //    return RedirectToAction(nameof(All));
        //}

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {

            var flightModel = await flightService.Details(Id);

            return View(flightModel);
        }


    }
}
