using Microsoft.AspNetCore.Mvc;
using static AirTicketApp.Areas.Admin.Constants.AdminConstants;
using Microsoft.AspNetCore.Authorization;

namespace AirTicketApp.Areas.Admin.Controllers
{   
    [Area(AreaName)]
    [Route("Admin/[controller]/[Action]/{id?}")]
    [Authorize(Roles = AdminRolleName)]

    public class BaseController : Controller
    {

    }
}
