using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirTicketApp.Areas.Admin.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> GiveAdminRole(string Id)
        {
            var result = await userService.GiveAdminRole(Id);
            var user = await userService.GetUserInfo(Id);
            TempData[MessageConstant.SuccessMessage] = $"User {user.UserName} is now Administrator!";
            return RedirectToAction("All", "User", new {Area="Admin"});
        }
        [HttpGet]
        public async Task<IActionResult> RemoveAdminRole(string Id)
        {
            var result = await userService.RemoveAdminRole(Id);
            var user = await userService.GetUserInfo(Id);
            TempData[MessageConstant.SuccessMessage] = $"User {user.UserName} is removed from administrator roles!";
            return RedirectToAction("All", "User", new { Area = "Admin" });
        }
    }
}