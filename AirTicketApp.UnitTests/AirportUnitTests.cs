namespace AirTicketApp.UnitTests
{
    public class AirportUnitTests
    {
        private IRepository repo;
        private IAirportService airportService;
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

            foreach (var id in DbContext.Airports.Select(e => e.Id))
            {
                var entity = new Airport { Id = id };
                DbContext.Airports.Attach(entity);
                DbContext.Airports.Remove(entity);
            }
            DbContext.SaveChanges();
        }

        [Test]
        public async Task TestGetAllAirports()
        {
            var repo = new Repository(DbContext);
            airportService = new AirportService(repo);

            await repo.AddRangeAsync(new List<Airport>
            {
                new Airport { Id = 100, Name="TestNameAirport1", IATACode="TS1" ,
                    City= new City(){ Id=100,Name="TestCity1",CountryId=1 }, },
                new Airport { Id = 200, Name="TestNameAirport2", IATACode="TS2" ,
                    City= new City(){ Id=200,Name="TestCity2",CountryId=2 }, },
                new Airport { Id = 300, Name="TestNameAirport3", IATACode="TS3" ,
                    City= new City(){ Id=300,Name="TestCity2",CountryId=2 }, },
                new Airport { Id = 400, Name="TestNameAirport4", IATACode="TS4" ,
                    City= new City(){ Id=400,Name="TestCity2",CountryId=2 }, },
            });
            await repo.SaveChangesAsync();

            var result = await airportService.GetAllAirports();

            Assert.That(result.Count(),Is.EqualTo(4));
            Assert.That(result.Any(a=>a.Id == 100), Is.True);
            Assert.That(result.Any(a=>a.Name == "TestNameAirport1"), Is.True);
            Assert.That(result.Any(a=>a.IATACode == "TS1"), Is.True);
            Assert.That(result.Any(a=>a.CityId == 100), Is.True);
        }

        public void TearDown()
        {
            DbContext.Dispose();
        }
    }
}
