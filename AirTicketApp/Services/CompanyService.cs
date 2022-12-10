using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models.CompanyModels;
using AirTicketApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirTicketApp.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IRepository repo;

        public CompanyService(IRepository repo)
        {
            this.repo = repo;
        }
        public async Task<IEnumerable<CompanyViewModel>> GetAllCompanies()
        {
            var companies = await repo.AllReadonly<Company>()
                .Select(c => new CompanyViewModel()
                {
                    Name = c.Name,
                    Id = c.Id,
                })
                .ToListAsync();

            return companies;
        }
    }
}
