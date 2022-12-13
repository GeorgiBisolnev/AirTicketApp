using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Models.FlightModels;
using AirTicketApp.Models.Ticket;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AirTicketApp.Extentions;
using AirTicketApp.Models.ApplicationUser;
using Newtonsoft.Json;

namespace AirTicketApp.Controllers
{
    [Authorize]
    public class Ticket : Controller
    {
        private readonly ITicketService ticketService;
        private readonly IFlightService flightService;
        private readonly IApplicationUserService userService;

        public Ticket(ITicketService _ticketService,
            IFlightService _flightService,
            IApplicationUserService _userService)
        {
            this.ticketService = _ticketService;
            this.flightService = _flightService;
            this.userService = _userService;
        }

        [HttpGet]
        public async Task<IActionResult> All(string userId)
        {
            var model =  await ticketService.AllTicketsByUser(userId);            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Buy(int Id)
        {
            TicketViewModel model = new TicketViewModel();
                try
                {
                    model.FlightModel = await flightService.Details(Id);
                }
                catch (Exception)
                {

                    TempData[MessageConstant.ErrorMessage] = "System error!";
                    return RedirectToAction("Index");
                }

            var userModel = await userService.GetUserInfo(User.Id());
            model.PhoneNumber = userModel.PhoneNumber;
            model.FirstName = userModel.FirstName;
            model.LastName = userModel.LastName;
            model.Email = userModel.Email;
            model.PassportNum = userModel.PassportNum;
            model.Country = userModel.Country;
            model.CountryId = userModel.CountryId;
            TempData["buyTicket"] = JsonConvert.SerializeObject(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(TicketViewModel model)
        {
            var modelFromTempData = JsonConvert
                .DeserializeObject<TicketViewModel>((string)TempData["buyTicket"]);

            var userModel = new ApplicationUserViewModel()
            {
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PassportNum = model.PassportNum,
                Email = model.Email,
            };
            bool success = await userService.SavePersonaUserInfo(userModel);
            var userId = User.Id();
            //var ticketsAvalibale = await ticketService.AvailableTickets(modelFromTempData.FlightModel.Id, modelFromTempData.FlightModel.Airplane.Capacity);
            if (success && true)
            {
                string ticketId = await ticketService.BuyTicket(modelFromTempData.FlightModel.Id, userId, modelFromTempData.FlightModel.Airplane.Capacity);
                TempData[MessageConstant.SuccessMessage] = "Successfully bued ticket number " + ticketId;
                return RedirectToAction("Index", "Home");
            }

            TempData[MessageConstant.ErrorMessage] = "System error!";
            return View(modelFromTempData.FlightModel.Id);
        }
    }
}
