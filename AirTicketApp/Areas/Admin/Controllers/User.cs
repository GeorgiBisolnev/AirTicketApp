using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AirTicketApp.Areas.Admin.Controllers
{
    public class User : BaseController
    {
        private readonly IApplicationUserService userService;

        public User(IApplicationUserService _userService)
        {
            this.userService = _userService;
        }
        public async Task<IActionResult> All()
        {
            var users = await userService.GetAll();

            return View(users);
        }
    }
}