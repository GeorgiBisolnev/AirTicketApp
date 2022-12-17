using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AirTicketApp.UnitTests
{
    public class CompanyUnitTests
    {
        private IRepository repo;
        private ICompanyService companyService;
        private AirTicketAppContext DbContext;

        [SetUp]
        public void Setup()
        {
            var contextOptions = new DbContextOptionsBuilder<AirTicketAppContext>()
                .UseInMemoryDatabase("FlightDB")
                .Options;

            DbContext = new AirTicketAppContext(contextOptions);

            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();

            foreach (var id in DbContext.Companies.Select(e => e.Id))
            {
                var entity = new Company { Id = id };
                DbContext.Companies.Attach(entity);
                DbContext.Companies.Remove(entity);
            }
            DbContext.SaveChanges();
        }
        
        [Test]
        public async Task TestGetCompanyById()
        {
            var repo = new Repository(DbContext);
            companyService = new CompanyService(repo);           

            await repo.AddRangeAsync(new List<Company>
            {
                new Company(){
                    Id=60000,
                    Name="Test Company",
                    ImgUrl="Img",
                    ImgUrlCarousel="ImgC"
                }
            });
            await repo.SaveChangesAsync();
            var result = await companyService.GetCompanyById(60000);
            Assert.That(result.Id, Is.EqualTo(60000));
            Assert.That(result.Name, Is.EqualTo("Test Company"));
            Assert.That(result.ImgUrl, Is.EqualTo("Img"));
            Assert.That(result.ImgUrlCarousel, Is.EqualTo("ImgC"));

        }
        [Test]
        public async Task TestGetCompanyByIdShouldReturnError()
        {
            var repo = new Repository(DbContext);
            companyService = new CompanyService(repo);

            Assert.ThrowsAsync<ArgumentException>(async () => await companyService.GetCompanyById(60001));
        }
        [Test]
        public async Task TestGetAllCompanies()
        {
            var repo = new Repository(DbContext);
            companyService = new CompanyService(repo);

            await repo.AddRangeAsync(new List<Company>
            {
                new Company(){
                    Id=1,
                    Name="Test Company1",
                    ImgUrl="Img1",
                    ImgUrlCarousel="ImgC1"
                },
                                new Company(){
                    Id=2,
                    Name="Test Company2",
                    ImgUrl="Img2",
                    ImgUrlCarousel="ImgC2"
                },
                new Company(){
                    Id=3,
                    Name="Test Company3",
                    ImgUrl="Img3",
                    ImgUrlCarousel="ImgC3"
                }
            });
            await repo.SaveChangesAsync();
            var result = await companyService.GetAllCompanies();
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(true, Is.EqualTo(result.Any(c => c.Name == "Test Company2")));
            Assert.That(true, Is.EqualTo(result.Any(c => c.ImgUrl == "Img2")));
            Assert.That(true, Is.EqualTo(result.Any(c => c.ImgUrlCarousel == "ImgC2")));

        }

        [Test]
        public async Task TestEdit()
        {
            var repo = new Repository(DbContext);
            companyService = new CompanyService(repo);

            await repo.AddRangeAsync(new List<Company>
            {
                new Company(){
                    Id=1,
                    Name="Test Company1",
                    ImgUrl="Img1",
                    ImgUrlCarousel="ImgC1"
                },
                new Company(){
                    Id=2,
                    Name="Test Company2",
                    ImgUrl="Img2",
                    ImgUrlCarousel="ImgC2"
                },
                new Company(){
                    Id=3,
                    Name="Test Company3",
                    ImgUrl="Img3",
                    ImgUrlCarousel="ImgC3"
                }
            });
            await repo.SaveChangesAsync();
            var result = await companyService.Edit(new Models.CompanyModels.CompanyViewModel {
                Id = 3,
                Name="edit3",
                ImgUrl="editimg",
                ImgUrlCarousel="testeditimgc"
            });
            Assert.That(result.Id, Is.EqualTo(3));
            Assert.That(result.Name, Is.EqualTo("edit3"));
            Assert.That(result.ImgUrl, Is.EqualTo("editimg"));
            Assert.That(result.ImgUrlCarousel, Is.EqualTo("testeditimgc"));
        }
        [Test]
        public async Task TestEditShouldReturnError()
        {
            var repo = new Repository(DbContext);
            companyService = new CompanyService(repo);

            await repo.AddRangeAsync(new List<Company>
            {
                new Company(){
                    Id=1,
                    Name="Test Company1",
                    ImgUrl="Img1",
                    ImgUrlCarousel="ImgC1"
                },
                new Company(){
                    Id=2,
                    Name="Test Company2",
                    ImgUrl="Img2",
                    ImgUrlCarousel="ImgC2"
                },
                new Company(){
                    Id=3,
                    Name="Test Company3",
                    ImgUrl="Img3",
                    ImgUrlCarousel="ImgC3"
                }
            });
            await repo.SaveChangesAsync();
            Assert.ThrowsAsync<ArgumentException>(async () => await companyService.Edit(new Models.CompanyModels.CompanyViewModel
            {
                Id = 4,
                Name = "edit3",
                ImgUrl = "editimg",
                ImgUrlCarousel = "testeditimgc"
            }));
        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
        }
    }
}
