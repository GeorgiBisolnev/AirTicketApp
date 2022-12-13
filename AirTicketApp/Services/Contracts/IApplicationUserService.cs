using AirTicketApp.Models.ApplicationUser;

namespace AirTicketApp.Services.Contracts
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserViewModel> GetUserInfo(string Id);

        Task<bool> SavePersonaUserInfo(ApplicationUserViewModel user);
    }
}
