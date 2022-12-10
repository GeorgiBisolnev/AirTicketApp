using AirTicketApp.Models.CompanyModels;

namespace AirTicketApp.Services.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyViewModel>> GetAllCompanies();
    }
}
