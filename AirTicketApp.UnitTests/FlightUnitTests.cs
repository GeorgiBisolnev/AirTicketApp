using AirTicketApp.Data;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTicketApp.UnitTests
{
    public class FlightUnitTests
    {
        private IRepository repo;
        private IFlightService flightService;
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
        }
        [Test]
        public async Task TestGetMostCheapThreeFlightsInMemory()
        {
            var repo=new Repository(DbContext);
            flightService = new FlightService(repo);

            DateTime datePast = DateTime.Parse("2022-11-19 16:30:00");
            DateTime dateTmr = DateTime.Now.AddDays(1);

            await repo.AddRangeAsync(new List<Flight>
            {
                new Flight()
                    { 
                        Id = 100,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 200,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                new Flight()
                    {
                        Id = 200,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 100,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                new Flight()
                    {
                        Id = 300,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 300,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                new Flight()
                    {
                        Id = 400,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 400,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                                new Flight()
                    {
                        Id = 401,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = datePast.AddHours(2),
                        DepartureDate = datePast,
                        CompanyId = 1,
                        Price = 5,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                });

            await repo.SaveChangesAsync();
            var flightCollection = await flightService.GetMostCheapThreeFlights();

            Assert.That(3, Is.EqualTo(flightCollection.Count()));
            Assert.That(flightCollection.Any(f=>f.Price==400), Is.False);
            Assert.That(flightCollection.Any(f=>f.Price==5), Is.False);
        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
        }
    }
}
