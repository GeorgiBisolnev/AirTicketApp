using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels.IdentityModels;
using AirTicketApp.Models.ApplicationUser;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirTicketApp.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IRepository repo;
        public ApplicationUserService(UserManager<ApplicationUser> _userManager, IRepository _repo)
        {
            this.userManager = _userManager;
            this.repo = _repo;
        }
        public async Task<ApplicationUserViewModel> GetUserInfo(string Id)
        {
            var user = await repo.AllReadonly<ApplicationUser>()
                .Where(u=>u.Id==Id)
                .Include(c=>c.Country)
                .Select(u=> new ApplicationUserViewModel()
                {
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PassportNum = u.PassportNum,
                    Email = u.Email,
                    Country = u.Country,
                    CountryId = u.CountryId,
                    PhoneNumber = u.PhoneNumber,
                })
                .FirstOrDefaultAsync();

            if (user !=null)
            {
                return user;
            }

            else throw new ArgumentException("There is no such User by given Id " + Id);
                
        }

        public async Task<bool> SavePersonaUserInfo(ApplicationUserViewModel user)
        {
            var findUser = await repo.AllReadonly<ApplicationUser>()
                .Where(u => u.NormalizedEmail == user.Email.ToUpper())
                .FirstOrDefaultAsync();

            if (findUser!=null)
            {
                findUser.FirstName = user.FirstName;
                findUser.LastName = user.LastName;
                findUser.PhoneNumber = user.PhoneNumber;
                findUser.PassportNum = user.PassportNum;

                repo.Update(findUser);
                await repo.SaveChangesAsync();
                return true;
            }

            else return false;
        }
    }
}
