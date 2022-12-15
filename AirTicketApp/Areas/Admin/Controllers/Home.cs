using AirTicketApp.Areas.Admin.Models;
using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirTicketApp.Areas.Admin.Controllers
{
    [Authorize]
    public class Home : BaseController
    {
        private readonly IFlightService flightService;
        private readonly IApplicationUserService userService;
        private readonly IAirportService airportService;
        private readonly ITicketService ticketService;


        public Home(ITicketService ticketService, IFlightService flightService, IApplicationUserService _userService, IAirportService airportService, ICompanyService companyService)
        {
            this.flightService = flightService;
            this.userService = _userService;
            this.airportService = airportService;
;           this.ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {          
            try
            {
                var model = new AdminBasicStatisticsViewModel();
                model.NumberOfUsers = await userService.NumberOfUsers();
                model.TotalTickets = await ticketService.TotalTickets();
                model.MostPopularAirport = await ticketService.MostPopularAirport();
                return View(model);
            }
            catch (Exception)
            {

                TempData[MessageConstant.ErrorMessage] = "System error!";
                var model = new AdminBasicStatisticsViewModel();
                return View(model);
            }
        }
    }
}
