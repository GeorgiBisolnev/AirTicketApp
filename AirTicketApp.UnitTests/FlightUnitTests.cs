using AirTicketApp.Data;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models.FlightModels;
using AirTicketApp.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private ITicketService ticketService;
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
            var repo = new Repository(DbContext);
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
            Assert.That(flightCollection.Any(f => f.Price == 400), Is.False);
            //Следващият полет е с най-сника цена, но е със стара дата и не трябва да влиза в изчисленията
            Assert.That(flightCollection.Any(f => f.Price == 5), Is.False);
        }
        
        [Test]
        public async Task TestGetMostCheapTheeFlightsInMemoryShouldReturn2models()
        {
            var repo = new Repository(DbContext);
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

            Assert.That(2, Is.EqualTo(flightCollection.Count()));
            //Следващият полет е с най-сника цена, но е със стара дата и не трябва да влиза в изчисленията
            Assert.That(flightCollection.Any(f => f.Price == 5), Is.False);
        }
        
        [Test]
        public async Task TestMostExpensiveDestination()
        {
            var repo = new Repository(DbContext);
            flightService = new FlightService(repo);

            DateTime datePast = DateTime.Parse("2022-11-19 16:30:00");
            DateTime dateTmr = DateTime.Now.AddDays(1);

            await repo.AddRangeAsync(new List<Flight>
            {
                new Flight()
                    {
                        Id = 1,
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
                        Id = 2,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = (decimal)200.011,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                new Flight()
                    {
                        Id = 3,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = datePast.AddHours(2),
                        DepartureDate = datePast,
                        CompanyId = 1,
                        Price = 500,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                });
            await repo.SaveChangesAsync();
            var result = await flightService.MostExpensiveDestination();
            //return $"{result.ArrivalCity}, {result.ArrivalAirport} for ${result.Price.ToString("F2")}"
            Assert.That("Moscow, Sheremetyevo Airport for $200.01", Is.EqualTo(result));

        }
        [Test]
        public async Task TestMostExpensiveDestinationShouldReturnEmptyString()
        {
            var repo = new Repository(DbContext);
            flightService = new FlightService(repo);

            DateTime datePast = DateTime.Parse("2022-11-19 16:30:00");
            DateTime dateTmr = DateTime.Now.AddDays(1);

            await repo.AddRangeAsync(new List<Flight>
            {

                });
            await repo.SaveChangesAsync();
            var result = await flightService.MostExpensiveDestination();
            //return $"{result.ArrivalCity}, {result.ArrivalAirport} for ${result.Price.ToString("F2")}"
            Assert.That(string.Empty, Is.EqualTo(result));

        }

        [Test]
        public async Task TestFlightExists()
        {
            var repo = new Repository(DbContext);
            flightService = new FlightService(repo);

            DateTime datePast = DateTime.Parse("2022-11-19 16:30:00");
            DateTime dateTmr = DateTime.Now.AddDays(1);

            await repo.AddRangeAsync(new List<Flight>
            {
                new Flight()
                    {
                        Id = 1,
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
                        Id = 2,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = (decimal)200.011,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                new Flight()
                    {
                        Id = 3,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = datePast.AddHours(2),
                        DepartureDate = datePast,
                        CompanyId = 1,
                        Price = 500,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                });
            await repo.SaveChangesAsync();
            var result = await flightService.FlightExists(3);
            var result1 = await flightService.FlightExists(2);
            var result2 = await flightService.FlightExists(5);
            var result3 = await flightService.FlightExists(0);
            
            Assert.That(result, Is.EqualTo(true));
            Assert.That(result1, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(false));
            Assert.That(result3, Is.EqualTo(false));

        }

        [Test]
        public async Task TestGetFlightById()
        {
            var repo = new Repository(DbContext);
            flightService = new FlightService(repo);

            DateTime datePast = DateTime.Parse("2022-11-19 16:30:00");
            DateTime dateTmr = DateTime.Now.AddDays(1);

            await repo.AddRangeAsync(new List<Flight>
            {
                new Flight()
                    {
                        Id = 1,
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
                        Id = 2,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = (decimal)200.011,
                        AirplaneId = 2,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                new Flight()
                    {
                        Id = 3,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = datePast.AddHours(2),
                        DepartureDate = datePast,
                        CompanyId = 1,
                        Price = 500,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                });
            await repo.SaveChangesAsync();
            var result = await flightService.GetFlightById(2);
            var result1 = await flightService.GetFlightById(3);
            Exception ex = Assert.ThrowsAsync<ArgumentException>(async ()=> await flightService.GetFlightById(4));
            //var obj1 = JsonConvert.SerializeObject(result);
            //var obj2 = JsonConvert.SerializeObject(compare);

            Assert.That(result.Id, Is.EqualTo(2));
            Assert.That(result1.Id, Is.EqualTo(3));
            Assert.That(result.DepartureAirport.Id, Is.EqualTo(1));
            Assert.That(result.ArrivalAirport.Id, Is.EqualTo(2));
            Assert.That(result.DepartureDate, Is.EqualTo(dateTmr));
            Assert.That(result.ArrivalDate, Is.EqualTo(dateTmr.AddHours(2)));
            Assert.That(result.CompanyId, Is.EqualTo(1));
            Assert.That(result.Price, Is.EqualTo((decimal)200.011));
            Assert.That(result.Airplane.Id, Is.EqualTo(2));
            Assert.That(result.Snack, Is.EqualTo(true));
            Assert.That(result.Food, Is.EqualTo(true));
            Assert.That(result.Luggage, Is.EqualTo(true));
            Assert.That(result.Duration, Is.EqualTo(120));          

        }

        [Test]
        public async Task TestBuyedFlightsByGivenFlightId()
        {
            var repo = new Repository(DbContext);
            flightService = new FlightService(repo);
            ticketService = new TicketService(repo, flightService);

            DateTime datePast = DateTime.Parse("2022-11-19 16:30:00");
            DateTime dateTmr = DateTime.Now.AddDays(1);

            await repo.AddRangeAsync(new List<Flight>
            {
                new Flight()
                    {
                        Id = 1,
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
                        Id = 2,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = (decimal)200.011,
                        AirplaneId = 2,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                new Flight()
                    {
                        Id = 3,
                        DepartureId = 1,
                        ArrivalId = 2,
                        ArrivalDate = datePast.AddHours(2),
                        DepartureDate = datePast,
                        CompanyId = 1,
                        Price = 500,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                });
            await repo.SaveChangesAsync();
            await repo.AddRangeAsync(new List<Ticket>
            {
                new Ticket()
                {
                    guid  = Guid.Parse("61956A05-70F7-455E-40BE-08DADEAEA730"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
                new Ticket()
                {
                    guid  = Guid.Parse("71A76EC7-0DC9-40D8-40C3-08DADEAEA730"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
                new Ticket()
                {
                    guid  = Guid.Parse("84252ECC-46F1-4230-8EEB-08DADEAF1977"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
                new Ticket()
                {
                    guid  = Guid.Parse("6F633795-9970-402B-D084-08DADEC7258D"),
                    FlightId = 2,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
                new Ticket()
                {
                    guid  = Guid.Parse("2FFCD027-4577-4F5B-D086-08DADEC7258D"),
                    FlightId = 2,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
                new Ticket()
                {
                    guid  = Guid.Parse("16886281-0FF4-49A9-5E6F-08DADED1F0E7"),
                    FlightId = 3,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },


            });
            await repo.SaveChangesAsync();

            var result = await flightService.BuyedFlightsByGivenFlightId(2);            
            var result1 = await flightService.BuyedFlightsByGivenFlightId(4);            
            Assert.That(result, Is.EqualTo(true));
            Assert.That(result1, Is.EqualTo(false));

        }

        [Test]
        public async Task TestEdit()
        {
            var repo = new Repository(DbContext);
            flightService = new FlightService(repo);

            DateTime datePast = DateTime.Parse("2022-11-19 16:30:00");
            DateTime dateTmr = DateTime.Now.AddDays(1);

            await repo.AddRangeAsync(new List<Flight>
            {
                new Flight()
                    {
                        Id = 1,
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
                    }
                });
            await repo.SaveChangesAsync();
            var model = new FlightViewModel()
            {
                Id = 1,
                DepartureId = 2,
                ArrivalId = 3,
                ArrivalDate = dateTmr.AddHours(3),
                DepartureDate = dateTmr.AddHours(1),
                CompanyId = 2,
                Price = 101,
                AirplaneId = 2,
                Snack = false,
                Food = false,
                Luggage = false,
                Duration = 121
            };
            await flightService.Edit(1,model);
            var result = await flightService.GetFlightById(1);
            Assert.That(model.DepartureId, Is.EqualTo(result.DepartureAirport.Id));


        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
        }
    }
}
