using AirTicketApp.Models;

namespace AirTicketApp.Services.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyViewModel>> GetAllCompanies();
    }
}
