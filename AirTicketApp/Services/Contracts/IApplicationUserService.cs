using AirTicketApp.Models.ApplicationUser;

namespace AirTicketApp.Services.Contracts
{
    public interface IApplicationUserService
    {
        Task<ApplicationUserViewModel> GetUserInfo(string Id);

        Task<bool> SavePersonaUserInfo(ApplicationUserViewModel user);
        Task<bool> IsAdministrator(string Id);
        Task<bool> GiveAdminRole(string Id); 
        Task<bool> RemoveAdminRole(string Id);
        Task<IEnumerable<ApplicationUserViewModel>> GetAll();
        Task<int> NumberOfUsers();

    }
}
