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

        public async Task<bool> Edit(CompanyViewModel model)
        {
            try
            {
                var company = await repo.All<Company>()
                                .Where(c => c.Id == model.Id)
                                .FirstOrDefaultAsync();

                if (company == null)
                {
                    throw new ArgumentException("There is no such company in the DB!");
                }
                else
                {
                    company.Name = model.Name;
                    company.ImgUrl = model.ImgUrl;
                    company.ImgUrlCarousel=model.ImgUrlCarousel;
                    repo.Update(company);
                    await repo.SaveChangesAsync();
                    return true;
                }               
            }
            catch (Exception)
            {

                throw new Exception("System error!");
            }
            
        }

        public async Task<List<CompanyViewModel>> GetAllCompanies()
        {
            var companies = await repo.AllReadonly<Company>()
                .AsNoTracking()
                .Select(c => new CompanyViewModel()
                {
                    Name = c.Name,
                    Id = c.Id,
                    ImgUrl = c.ImgUrl,
                    ImgUrlCarousel = c.ImgUrlCarousel
                })
                .ToListAsync();

            return companies;
        }

        public Task<CompanyViewModel> GetCompanyById(int id)
        {
            var result = repo.AllReadonly<Company>()
                .AsNoTracking()
                .Select(c=> new CompanyViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImgUrl = c.ImgUrl,
                    ImgUrlCarousel= c.ImgUrlCarousel
                })
                .FirstOrDefaultAsync(c=>c.Id==id);

            if (result==null)
            {
                throw new ArgumentException ("There is no company with such Id");
            }else
                {
                    return result;
                }
        }
    }
}
