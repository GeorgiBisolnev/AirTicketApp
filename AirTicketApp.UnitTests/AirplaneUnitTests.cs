using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicketApp.UnitTests
{
    public class AirplaneUnitTests
    {
        private IRepository repo;
        private IAirplaneService airplaneService;
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

            foreach (var id in DbContext.Airplanes.Select(e => e.Id))
            {
                var entity = new Airplane { Id = id };
                DbContext.Airplanes.Attach(entity);
                DbContext.Airplanes.Remove(entity);
            }
            DbContext.SaveChanges();
        }

        [Test]
        public async Task TestGetAllAirplanes()
        {
            var repo = new Repository(DbContext);
            airplaneService = new AirplaneService(repo);

            await repo.AddRangeAsync(new List<Airplane>
            {
                new Airplane() {
                    Id = 1000,
                    Model="TestModel",
                    Manufacture = new Manufacture()
                    {
                        Id=1000,
                        Name="TestManufacture1"
                    },
                    Capacity=100
                },
                new Airplane() {
                    Id = 2000,
                    Model="TestModel2",
                    Manufacture = new Manufacture()
                    {
                        Id=2000,
                        Name="TestManufacture12"
                    },
                    Capacity=102
                },
                new Airplane() {
                    Id = 3000,
                    Model="TestModel3",
                    Manufacture = new Manufacture()
                    {
                        Id=3000,
                        Name="TestManufacture3"
                    },
                    Capacity=103
                },
            });
            await repo.SaveChangesAsync();

            var result = await airplaneService.GetAllAirplanes();

            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.Any(a => a.Id == 1000), Is.True);
            Assert.That(result.Any(a => a.Model == "TestModel3"), Is.True);
            Assert.That(result.Any(a => a.Manufacture.Name == "TestManufacture3"), Is.True);
            Assert.That(result.Any(a => a.Capacity == 102), Is.True);
        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
        }
    }
}
