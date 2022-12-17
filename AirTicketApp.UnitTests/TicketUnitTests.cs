namespace AirTicketApp.UnitTests
{
    public class TicketUnitTests
    {
        private IRepository repo;
        private IFlightService flightService;
        private ITicketService ticketService;
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
        public async Task TestTotalTickets()
        {
            var repo = new Repository(DbContext);
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
            var result = await ticketService.TotalTickets();

            Assert.AreEqual(6, result);
        }
        [Test]
        public async Task TestMostPopularAirport()
        {
            var repo = new Repository(DbContext);
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
                        ArrivalAirport = new Airport()
                        {
                            Id=50000,
                            Name = "Test",
                            IATACode="tst",
                            CityId=1,
                        },
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
                        ArrivalId = 3,
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

            var result = await ticketService.MostPopularAirport();
            Assert.AreEqual("", result);

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
            result = await ticketService.MostPopularAirport();
            Assert.AreEqual("Test", result);
        }
        [Test]
        public async Task TestBuyTicket()
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
                        ArrivalId = 50000,
                        ArrivalAirport = new Airport()
                        {
                            Id=50000,
                            Name = "Test",
                            IATACode="tst",
                            CityId=1,
                        },
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 100,
                        AirplaneId = 50000,
                        Airplane = new Airplane()
                        {
                            Id = 50000,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                        AirplaneId = 2,
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
                    guid  = Guid.Parse("16886281-0FF4-49A9-5E6F-08DADED1F0E7"),
                    FlightId = 3,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },


            });
            await repo.SaveChangesAsync();
            var result = await ticketService.BuyTicket(1, "dea12856-c198-4129-b3f3-b893d8395082",4);

            Assert.AreNotEqual("", result);
        }
        [Test]
        public async Task TestBuyTicketShuldReturnArgumentException()
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
                        ArrivalId = 50000,
                        ArrivalAirport = new Airport()
                        {
                            Id=50000,
                            Name = "Test",
                            IATACode="tst",
                            CityId=1,
                        },
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 100,
                        AirplaneId = 50000,
                        Airplane = new Airplane()
                        {
                            Id = 50000,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                        AirplaneId = 50001,
                        Airplane = new Airplane()
                        {
                            Id = 50001,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                    guid  = Guid.Parse("16886281-0FF4-49A9-5E6F-08DADED1F0E7"),
                    FlightId = 3,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },


            });
            await repo.SaveChangesAsync();
            var result = await ticketService.BuyTicket(1, "dea12856-c198-4129-b3f3-b893d8395082", 4);

            Assert.ThrowsAsync<ArgumentException>(async () => await ticketService.BuyTicket(1, "dea12856-c198-4129-b3f3-b893d8395082", 4));
            Assert.ThrowsAsync<ArgumentException>(async () => await ticketService.BuyTicket(112548, "dea12856-c198-4129-b3f3-b893d8395082", 4));
            Assert.ThrowsAsync<ArgumentException>(async () => await ticketService.BuyTicket(3, "dea12856-c198-4129-b3f3-b893d8395082", 4));
        }
        [Test]
        public async Task TestAvailableTickets()
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
                        ArrivalId = 50000,
                        ArrivalAirport = new Airport()
                        {
                            Id=50000,
                            Name = "Test",
                            IATACode="tst",
                            CityId=1,
                        },
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 100,
                        AirplaneId = 50000,
                        Airplane = new Airplane()
                        {
                            Id = 50000,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                        AirplaneId = 50001,
                        Airplane = new Airplane()
                        {
                            Id = 50001,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                    guid  = Guid.Parse("16886281-0FF4-49A9-5E6F-08DADED1F0E7"),
                    FlightId = 3,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },


            });
            await repo.SaveChangesAsync();
            var result = await ticketService.AvailableTickets(1,4);

            Assert.AreEqual(true, result);
        }
        [Test]
        public async Task TestAvailableTicketsShouldReturnFalse()
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
                        ArrivalId = 50000,
                        ArrivalAirport = new Airport()
                        {
                            Id=50000,
                            Name = "Test",
                            IATACode="tst",
                            CityId=1,
                        },
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 100,
                        AirplaneId = 50000,
                        Airplane = new Airplane()
                        {
                            Id = 50000,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                        AirplaneId = 50001,
                        Airplane = new Airplane()
                        {
                            Id = 50001,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                    guid  = Guid.Parse("16886281-0FF4-49A9-5E6F-08DADED1F0E7"),
                    FlightId = 3,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },


            });

            await repo.AddRangeAsync(new List<Ticket>
            {
                new Ticket()
                {
                    guid  = Guid.Parse("61956A05-70AA-455E-40BE-08DADEAEA730"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
            });

            await repo.SaveChangesAsync();

            var result = await ticketService.AvailableTickets(1, 4);

            Assert.AreEqual(false, result);


        }
        [Test]
        public async Task TestAllTicketsByUser()
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
                        DepartureId = 60000,
                        DepartureAirport = new Airport()
                        {
                            Id=60000,
                            Name = "TestD",
                            IATACode="TSD",
                            CityId=2,
                        },
                        ArrivalId = 50000,
                        ArrivalAirport = new Airport()
                        {
                            Id=50000,
                            Name = "Test",
                            IATACode="tst",
                            CityId=1,
                        },
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr,
                        CompanyId = 1,
                        Price = 100,
                        AirplaneId = 50000,
                        Airplane = new Airplane()
                        {
                            Id = 50000,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                        AirplaneId = 50001,
                        Airplane = new Airplane()
                        {
                            Id = 50001,
                            ManufactureId=2,
                            Model="Skyhawk 172",
                            Capacity=4
                        },
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
                    guid  = Guid.Parse("16886281-0FF4-49A9-5E6F-08DADED1F0E7"),
                    FlightId = 3,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395081"
                },


            });
            await repo.SaveChangesAsync();

            var result = await ticketService.AllTicketsByUser("dea12856-c198-4129-b3f3-b893d8395082");

            Assert.That(result.Any(t=>t.TicketId.ToUpper()=="61956A05-70F7-455E-40BE-08DADEAEA730"), Is.EqualTo(true));
            Assert.That(result.Any(t=>t.TicketId.ToUpper()=="71A76EC7-0DC9-40D8-40C3-08DADEAEA730"), Is.EqualTo(true));
            Assert.That(result.Any(t=>t.TicketId.ToUpper()=="84252ECC-46F1-4230-8EEB-08DADEAF1977"), Is.EqualTo(true));

            Assert.That("TestD", Is.EqualTo(result.First(t => t.FlightId == 1).DepartureAirportName));
            Assert.That("TSD", Is.EqualTo(result.First(t => t.FlightId == 1).DepartureAirportCode));
            Assert.That("Moscow", Is.EqualTo(result.First(t => t.FlightId == 1).DepartureCity));
            Assert.That("Russia", Is.EqualTo(result.First(t => t.FlightId == 1).DepartureCountry));
            Assert.That("Test", Is.EqualTo(result.First(t => t.FlightId == 1).ArrivalAirportName));
            Assert.That("tst", Is.EqualTo(result.First(t => t.FlightId == 1).ArrivalAirportCode));
            Assert.That("Sofia", Is.EqualTo(result.First(t => t.FlightId == 1).ArrivalCity));
            Assert.That("Bulgaria", Is.EqualTo(result.First(t => t.FlightId == 1).ArrivalCountry));
            Assert.That("Bulgaria Air", Is.EqualTo(result.First(t => t.FlightId == 1).CompanyName));
            Assert.That(dateTmr.AddHours(2), Is.EqualTo(result.First(t => t.FlightId == 1).ArrivalDate));
            Assert.That(dateTmr, Is.EqualTo(result.First(t => t.FlightId == 1).DepartureDate));
            Assert.That(100, Is.EqualTo(result.First(t => t.FlightId == 1).Price));
        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
        }
    }
}
