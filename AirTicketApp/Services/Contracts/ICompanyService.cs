using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models.CompanyModels;

namespace AirTicketApp.Services.Contracts
{
    public interface ICompanyService
    {
        Task<List<CompanyViewModel>> GetAllCompanies();
        Task<CompanyViewModel> GetCompanyById(int id);
        Task<Company> Edit(CompanyViewModel model);
        Task<Company> Add(CompanyViewModel model);
    }
}
