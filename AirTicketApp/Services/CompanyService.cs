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
        /// <summary>
        /// Процедурата редактира информация за авиокомпания като в параметрите се подава модел,
        /// от който се взима информация за редакция
        /// </summary>
        /// <param name="model"></param>
        /// <returns>При успешна редакция връща булева стойност true</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<Company> Edit(CompanyViewModel model)
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
                    return company;
                }            
        }
        /// <summary>
        /// Процедурата връща списъчен модел с всички авиокомпании
        /// </summary>
        /// <returns>Списък с всички авиокомпании</returns>
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
                .OrderBy(comparer => comparer.Name)
                .ToListAsync();

            return companies;
        }
        /// <summary>
        /// Процедурата връща един модел на авиокомпания по зададено Id в параметрите на процедурата 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Връща модел на авиокомпания</returns>
        /// <exception cref="ArgumentException"></exception>
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

            if (result.Result==null)
            {
                throw new ArgumentException ("There is no company with such Id");
            }else
                {
                    return result;
                }
        }
        /// <summary>
        /// Процедурата добавя нова авиокомпания към БД
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Company> Add(CompanyViewModel model)
        {
            var company = await repo.All<Company>()
                            .Where(c => c.Name == model.Name)
                            .FirstOrDefaultAsync();

            if (company == null)
            {
                var newCompany = new Company()
                {
                    Name = model.Name,
                    ImgUrl = model.ImgUrl,
                    ImgUrlCarousel = model.ImgUrlCarousel
                };
                await repo.AddAsync(newCompany);

                await repo.SaveChangesAsync();
                return newCompany;
            }
            else
            {
                throw new ArgumentException($"Airline company with name {model.Name} already exists!");
            }
        }
    }
}
