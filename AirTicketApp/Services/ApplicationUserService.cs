﻿using AirTicketApp.Data.Common.Repository;
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
        /// <summary>
        /// Процедурата прочита всички потребители от БД и ги връща като списък
        /// </summary>
        /// <returns>Връща списък с модел на потребител</returns>
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
            repo.DetachAllEntities();
            return users;
        }
        /// <summary>
        /// Процедурата проверява дали потребителят е администратор по ID на потребител
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Връща булева стойност true, ако потребителя е администратор</returns>
        public async Task<bool> IsAdministrator(string Id)
        {
            var user = await repo.AllReadonly<ApplicationUser>()
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
            repo.DetachAllEntities();
            return false;
        }
        /// <summary>
        /// Процедурата чете от БД информация за потребител, като търсенето се извършва по Id на потребител
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Връща модел на потребител</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ApplicationUserViewModel> GetUserInfo(string Id)
        {
            var user = await repo.AllReadonly<ApplicationUser>()
                //.AsNoTracking()
                .Where(u=>u.Id==Id)
                .Include(c=>c.Country)
                .Select(u=> new ApplicationUserViewModel()
                {
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PassportNum = u.PassportNum,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                })
                .FirstOrDefaultAsync();
            repo.DetachAllEntities();
            if (user !=null)
            {
                return user;
            }

            else throw new ArgumentException("There is no such User by given Id " + Id);
                
        }
        /// <summary>
        /// Процедурата запазва допълнителните полета добавени към IdentityUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Въща булева стойност true при успешно запазване на данните</returns>
        public async Task<bool> SavePersonaUserInfo(ApplicationUserViewModel user)
        {
            var findUser = await repo.AllReadonly<ApplicationUser>()
                //.AsNoTracking()
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
                repo.DetachAllEntities();
                return true;
            }

            else return false;
        }
        /// <summary>
        /// Процедурата дава административна роля на потребител по зададено ID на потребител
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>въща булева стойност true при успешно даване на право на роля</returns>
        public async Task<bool> GiveAdminRole(string Id)
        {
            var user = await repo.All<ApplicationUser>()
                //.AsNoTracking()
                .FirstOrDefaultAsync(u=>u.Id == Id);

            if (user!=null)
            {                
                await userManager.AddToRoleAsync(user, AdminRolleName);
                return true;
            }
            repo.DetachAllEntities();
            return false;
        }
        public async Task<bool> RemoveAdminRole(string Id)
        {
            repo.DetachAllEntities();
            var user = await repo.All<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.Id == Id);

            if (user != null)
            {
                await userManager.RemoveFromRoleAsync(user, AdminRolleName);
                return true;
            }

            return false;
        }
        /// <summary>
        /// Процедурата връща броя на регистрираните потребители в БД
        /// </summary>
        /// <returns></returns>
        public async Task<int> NumberOfUsers()
        {
            return await repo.AllReadonly<ApplicationUser>()
                //.AsNoTracking()
                .CountAsync();

        }
    }
}
