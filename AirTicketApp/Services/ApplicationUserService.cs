using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels.IdentityModels;
using AirTicketApp.Models.ApplicationUser;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static AirTicketApp.Areas.Admin.Constants.AdminConstants;

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

        public async Task<IEnumerable<ApplicationUserViewModel>> GetAll()
        {
            var users = await repo.AllReadonly<ApplicationUser>()
                .AsNoTracking()
                .Include(c => c.Country)
                .Select(u => new ApplicationUserViewModel()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PassportNum = u.PassportNum,
                    Email = u.Email,
                    Country = u.Country,
                    CountryId = u.CountryId,
                    PhoneNumber = u.PhoneNumber,
                })
                .ToListAsync();

            foreach (var user in users)
            {
                if (await IsAdministrator(user.Id))
                {
                    user.IsAdministrator = true;
                }
                else
                    user.IsAdministrator = false;
            }
            
            return users;
        }
        public async Task<bool> IsAdministrator(string Id)
        {
            var user = await repo.AllReadonly<ApplicationUser>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == Id);

            if (user ==null)
            {
                return false;
            }

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                if (role==AdminRolleName)
                {
                    return true;
                }
            }

            return false;
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

        public async Task<bool> GiveAdminRole(string Id)
        {
            var user = await repo.AllReadonly<ApplicationUser>()
                .FirstOrDefaultAsync(u=>u.Id == Id);

            if (user!=null)
            {
                await userManager.AddToRoleAsync(user, AdminRolleName);
                return true;
            }

            return false;
        }
    }
}
