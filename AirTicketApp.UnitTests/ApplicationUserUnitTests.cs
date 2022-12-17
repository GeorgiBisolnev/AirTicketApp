using AirTicketApp.Data.EntityModels.IdentityModels;
using AirTicketApp.Models.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace AirTicketApp.UnitTests
{
    public class ApplicationUserUnitTests
    {
        private IRepository repo;
        private IApplicationUserService userService;
        private UserManager<ApplicationUser> userManager;
        private AirTicketAppContext DbContext;

        [SetUp]
        public void Setup()
        {
            var contextOptions = new DbContextOptionsBuilder<AirTicketAppContext>()
                .UseInMemoryDatabase("DB")
                .Options;

            DbContext = new AirTicketAppContext(contextOptions);

            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();

        }

        [Test]
        public async Task TestGetUserInfo()
        {
            var repo = new Repository(DbContext);
            userService = new ApplicationUserService(userManager, repo);

            await repo.AddRangeAsync(CreateUsers());
            await repo.SaveChangesAsync();

            var result = await userService.GetUserInfo("08c96b99-96ad-46d2-8a8a-189ab99634a9");

            Assert.That(result.UserName, Is.EqualTo("user1@gmail.com"));
            Assert.That(result.FirstName, Is.EqualTo("Name1"));
            Assert.That(result.LastName, Is.EqualTo("LastName1"));
            Assert.That(result.PassportNum, Is.EqualTo("S123451"));
            Assert.That(result.Email, Is.EqualTo("user1@gmail.com"));
            Assert.That(result.PhoneNumber, Is.EqualTo("0882274281"));
        }

        [Test]
        public async Task TestGetUserInfoShouldreturnArgumentException()
        {
            var repo = new Repository(DbContext);
            userService = new ApplicationUserService(userManager, repo);

            await repo.AddRangeAsync(CreateUsers());
            await repo.SaveChangesAsync();

            Assert.ThrowsAsync<ArgumentException>(async ()=> await userService.GetUserInfo("N/a"));
        }

        [Test]
        public async Task TestSavePersonaUserInfo()
        {
            var repo = new Repository(DbContext);
            userService = new ApplicationUserService(userManager, repo);

            var model = new  ApplicationUserViewModel(){
                FirstName = "Name1-",
                LastName = "LastName1-",
                PassportNum = "S1234510",                
                PhoneNumber = "0882274271",
                Email= "user@gmail.com"
            };

            await userService.SavePersonaUserInfo(model);
            var result = await userService.GetUserInfo("dea12856-c198-4129-b3f3-b893d8395082");
            
            Assert.That(result.FirstName, Is.EqualTo(model.FirstName));
            Assert.That(result.LastName, Is.EqualTo(model.LastName));
            Assert.That(result.PassportNum, Is.EqualTo(model.PassportNum));
            Assert.That(result.PhoneNumber, Is.EqualTo(model.PhoneNumber));

        }

        [Test]
        public async Task TestSavePersonaUserInfoShuldReturnFalse()
        {
            var repo = new Repository(DbContext);
            userService = new ApplicationUserService(userManager, repo);

            var model = new ApplicationUserViewModel()
            {
                FirstName = "Name1-",
                LastName = "LastName1-",
                PassportNum = "S1234510",
                PhoneNumber = "0882274271",
                Email = "N/a"
            };

            var result = await userService.SavePersonaUserInfo(model);
            Assert.That(result, Is.EqualTo(false));

        }

        [Test]
        public async Task TestNumberOfUsers()
        {
            var repo = new Repository(DbContext);
            userService = new ApplicationUserService(userManager, repo);

            await repo.AddRangeAsync(CreateUsers());
            await repo.SaveChangesAsync();

            var result = await userService.NumberOfUsers();

            Assert.That(result, Is.EqualTo(3));
        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
        }

        private List<ApplicationUser> CreateUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var users = new List<ApplicationUser>();

            var user1 = new ApplicationUser()
            {
                Id = "08c96b99-96ad-46d2-8a8a-189ab99634a9",
                UserName = "user1@gmail.com",
                NormalizedUserName = "USER1@GMAIL.COM",
                Email = "user1@gmail.com",
                NormalizedEmail = "USER1@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Name1",
                LastName = "LastName1",
                PassportNum = "S123451",
                CountryId = 1,
                PhoneNumber = "0882274281",
            };

            user1.PasswordHash =
                 hasher.HashPassword(user1, "user123");
            var user2 = new ApplicationUser()
            {
                Id = "00cc5c50-7dba-4dd6-bfd4-6a5ac1100d8d",
                NormalizedUserName = "user2@gmail.com",
                Email = "user2@gmail.com",
                UserName = "user2@gmail.com",
                NormalizedEmail = "USER2@GMAIL.COM",
                EmailConfirmed = true,
                FirstName = "Name2",
                LastName = "LastName2",
                PassportNum = "S1234562",
                CountryId = 2,
                PhoneNumber = "08822742822",
            };

            user2.PasswordHash =
                 hasher.HashPassword(user2, "user123");

            users.Add(user1);
            users.Add(user2);

            return users;
        }
    }
}
