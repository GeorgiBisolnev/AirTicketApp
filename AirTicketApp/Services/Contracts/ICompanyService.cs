using AirTicketApp.Models.CompanyModels;

namespace AirTicketApp.Services.Contracts
{
    public interface ICompanyService
    {
        Task<List<CompanyViewModel>> GetAllCompanies();
        Task<CompanyViewModel> GetCompanyById(int id);
        Task<bool> Edit(CompanyViewModel model);
    }
}
