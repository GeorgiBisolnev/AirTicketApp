using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Extentions;
using AirTicketApp.Models.ApplicationUser;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirTicketApp.Controllers
{
    [Authorize]
    public class User : Controller
    {
        private readonly IApplicationUserService userService;

        public User(IApplicationUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var info = await  userService.GetUserInfo(User.Id());
            return View(info);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ApplicationUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.WarningMessage] = "Not all validations are passed!";
                return RedirectToAction("Index", "User");
            }
            try
            {
                var success = await userService.SavePersonaUserInfo(model);
                if (success)
                {
                    TempData[MessageConstant.SuccessMessage] = "Profile data updated!";
                    return View(model);
                }
                else
                {
                    TempData[MessageConstant.ErrorMessage] = "We cant save your profile now!";
                    return View(model);
                }
            }
            catch (Exception)
            {

                TempData[MessageConstant.ErrorMessage] = "System error!";
                return View(model);
            }
        }
    }
}
