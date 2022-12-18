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
                        CompanyId = 60000,
                        Company= new Company
                        {
                            Id=60000,
                            Name= "TestCompany",
                            ImgUrlCarousel= "ImgC",
                            ImgUrl="Img"
                        },
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
            Assert.That(flightCollection.Any(f => f.DepartureAirport == "Sofia Airport"), Is.True);
            Assert.That(flightCollection.Any(f => f.ArrivalAirport == "Sheremetyevo Airport"), Is.True);
            Assert.That(flightCollection.Any(f => f.DepartureCity == "Sofia"), Is.True);
            Assert.That(flightCollection.Any(f => f.ArrivalCity == "Moscow"), Is.True);
            Assert.That(flightCollection.Any(f => f.Company == "TestCompany"), Is.True);
            Assert.That(flightCollection.Any(f => f.ImgUrlC == "ImgC"), Is.True);
            //Следващият полет е с най-сника цена, но е със стара дата и не трябва да влиза в изчисленията
            Assert.That(flightCollection.Any(f => f.Price == 5), Is.False);
            Assert.That(flightCollection.Any(f => f.FlightId == 400), Is.False);
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
            Assert.That(model.ArrivalId, Is.EqualTo(result.ArrivalAirport.Id));
            Assert.That(model.ArrivalDate, Is.EqualTo(result.ArrivalDate));
            Assert.That(model.DepartureDate, Is.EqualTo(result.DepartureDate));
            Assert.That(model.CompanyId, Is.EqualTo(result.CompanyId));
            Assert.That(model.Price, Is.EqualTo(result.Price));
            Assert.That(model.AirplaneId, Is.EqualTo(result.Airplane.Id));
            Assert.That(model.Snack, Is.EqualTo(result.Snack));
            Assert.That(model.Luggage, Is.EqualTo(result.Luggage));
            Assert.That(model.Duration, Is.EqualTo(result.Duration));
        }

        [Test]
        public async Task TestEditWhenTicketsAreBuyedShouldReturnArgumentException()
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
            await repo.AddRangeAsync(new List<Ticket>
            {
                new Ticket()
                {
                    guid  = Guid.Parse("61956A05-70F7-455E-40BE-08DADEAEA730"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
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
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.Edit(1, model));

        }
        [Test]
        public async Task TestEditWhenModelIsNotPresentinDbShouldReturnArgumentException()
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
                Id = 100,
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

            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.Edit(100, model));

        }
        [Test]
        public async Task TestEditEverythingWhatIamallowedWhenTicketIsBuyed()
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
            await repo.AddRangeAsync(new List<Ticket>
            {
                new Ticket()
                {
                    guid  = Guid.Parse("61956A05-70F7-455E-40BE-08DADEAEA730"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
            });
            await repo.SaveChangesAsync();

            var model = new FlightViewModel()
            {
                Id = 1,
                DepartureId = 1,
                ArrivalId = 2,
                ArrivalDate = dateTmr.AddHours(2),
                DepartureDate = dateTmr,
                CompanyId = 1,
                Price = 1001,
                AirplaneId = 1,
                Snack = false,
                Food = false,
                Luggage = false,
                Duration = 121
            };

            Assert.DoesNotThrowAsync(async () => await flightService.Edit(1, model));
            //Assert.ThrowsAsync<ArgumentException>(async () => await flightService.Edit(1, model));
            var compare = await flightService.GetFlightById(1);
            Assert.That(compare.Id, Is.EqualTo(1));
            Assert.That(compare.DepartureAirport.Id, Is.EqualTo(1));
            Assert.That(compare.ArrivalAirport.Id, Is.EqualTo(2));
            Assert.That(compare.ArrivalDate, Is.EqualTo(dateTmr.AddHours(2)));
            Assert.That(compare.DepartureDate, Is.EqualTo(dateTmr));
            Assert.That(compare.CompanyId, Is.EqualTo(1));
            Assert.That(compare.Price, Is.EqualTo(1001));
            Assert.That(compare.Airplane.Id, Is.EqualTo(1));
            Assert.That(compare.Snack, Is.EqualTo(false));
            Assert.That(compare.Food, Is.EqualTo(false));
            Assert.That(compare.Luggage, Is.EqualTo(false));
            Assert.That(compare.Duration, Is.EqualTo(121));

        }
        [Test]
        public async Task TestEditDatesAndTimeWithMoreThan24hoursShuldReturnArgumentException()
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
            await repo.AddRangeAsync(new List<Ticket>
            {
                new Ticket()
                {
                    guid  = Guid.Parse("61956A05-70F7-455E-40BE-08DADEAEA730"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
            });
            await repo.SaveChangesAsync();

            var model = new FlightViewModel()
            {
                Id = 1,
                DepartureId = 1,
                ArrivalId = 2,
                ArrivalDate = dateTmr.AddHours(26).AddMinutes(1),
                DepartureDate = dateTmr,
                CompanyId = 1,
                Price = 100,
                AirplaneId = 1,
                Snack = true,
                Food = true,
                Luggage = true,
                Duration = 120
            };

            //Assert.DoesNotThrowAsync(async () => await flightService.Edit(1, model));
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.Edit(1, model));

            model = new FlightViewModel()
            {
                Id = 1,
                DepartureId = 1,
                ArrivalId = 2,
                ArrivalDate = dateTmr.AddHours(26),
                DepartureDate = dateTmr.AddHours(24).AddMinutes(1),
                CompanyId = 1,
                Price = 100,
                AirplaneId = 1,
                Snack = true,
                Food = true,
                Luggage = true,
                Duration = 120
            };
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.Edit(1, model));

            model = new FlightViewModel()
            {
                Id = 1,
                DepartureId = 1,
                ArrivalId = 2,
                ArrivalDate = dateTmr.AddHours(26),
                DepartureDate = dateTmr.AddHours(-24).AddMinutes(-1),
                CompanyId = 1,
                Price = 100,
                AirplaneId = 1,
                Snack = true,
                Food = true,
                Luggage = true,
                Duration = 120
            };
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.Edit(1, model));

            model = new FlightViewModel()
            {
                Id = 1,
                DepartureId = 1,
                ArrivalId = 2,
                ArrivalDate = dateTmr.AddHours(-22).AddMinutes(-1),
                DepartureDate = dateTmr.AddHours(-24),
                CompanyId = 1,
                Price = 100,
                AirplaneId = 1,
                Snack = true,
                Food = true,
                Luggage = true,
                Duration = 120
            };
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.Edit(1, model));


        }
        [Test]
        public async Task TestEditDatesAndTime()
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
            await repo.AddRangeAsync(new List<Ticket>
            {
                new Ticket()
                {
                    guid  = Guid.Parse("61956A05-70F7-455E-40BE-08DADEAEA730"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
            });
            await repo.SaveChangesAsync();

            var model = new FlightViewModel()
            {
                Id = 1,
                DepartureId = 1,
                ArrivalId = 2,
                ArrivalDate = dateTmr.AddHours(26),
                DepartureDate = dateTmr.AddHours(24),
                CompanyId = 1,
                Price = 100,
                AirplaneId = 1,
                Snack = true,
                Food = true,
                Luggage = true,
                Duration = 120
            };

            Assert.DoesNotThrowAsync(async () => await flightService.Edit(1, model));

        }

        [Test]
        public async Task TestAllFlightsFilter()
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
                        ArrivalDate = dateTmr.AddHours(2).AddMinutes(-1),
                        DepartureDate = dateTmr.AddMinutes(-1),
                        CompanyId = 2,
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
                        CompanyId = 3,
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
                        CompanyId = 4,
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
                        ArrivalDate = dateTmr.AddHours(2),
                        DepartureDate = dateTmr.AddDays(1),
                        CompanyId = 5,
                        Price = 400,
                        AirplaneId = 1,
                        Snack = true,
                        Food = true,
                        Luggage = true,
                        Duration = 120
                    },
                                new Flight()
                    {
                        Id = 402,
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
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.AllFlightsFilter());
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.AllFlightsFilter(0));
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.AllFlightsFilter(null));
            Assert.ThrowsAsync<ArgumentException>(async () => await flightService.AllFlightsFilter(0,null));
            var flightCollection = await flightService.AllFlightsFilter(0,datePast.Date,2,1);
            Assert.That(0, Is.EqualTo(flightCollection.Count()));
            flightCollection = await flightService.AllFlightsFilter(FlightSorting.Price, dateTmr.Date, 2, 1);
            Assert.That(4, Is.EqualTo(flightCollection.Count()));
            Assert.That(200, Is.EqualTo(flightCollection.FirstOrDefault().Id));
            flightCollection = await flightService.AllFlightsFilter(FlightSorting.Company, dateTmr.Date, 2, 1);
            Assert.That(200, Is.EqualTo(flightCollection.FirstOrDefault().Id));
            flightCollection = await flightService.AllFlightsFilter(FlightSorting.DepartureDate, dateTmr.Date, 2, 1);
            Assert.That(200, Is.EqualTo(flightCollection.FirstOrDefault().Id));
            flightCollection = await flightService.AllFlightsFilter(FlightSorting.ArrivalDate, dateTmr.Date, 2, 1);
            Assert.That(200, Is.EqualTo(flightCollection.FirstOrDefault().Id));
            flightCollection = await flightService.AllFlightsFilter(null, dateTmr.Date, 2, 1);
            Assert.That(100, Is.EqualTo(flightCollection.FirstOrDefault().Id));


        }
        [Test]
        public async Task TestCreate()
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
                });

            await repo.SaveChangesAsync();
            var count = repo.AllReadonly<Flight>().Count();
            Assert.That(1,Is.EqualTo(count));

            int newflightId = await flightService.Create(new FlightViewModel() {
                DepartureId = 1,
                ArrivalId = 2,
                ArrivalDate = dateTmr.AddHours(3),
                DepartureDate = dateTmr.AddHours(1),
                CompanyId = 2,
                Price = 5678,
                AirplaneId = 2,
                Snack = true,
                Food = false,
                Luggage = false,
                Duration = 125
            });
            await repo.SaveChangesAsync();            
            count = repo.AllReadonly<Flight>().Count();
            Assert.That(2, Is.EqualTo(count));

            var model = await flightService.GetFlightById(newflightId);

            Assert.That(1, Is.EqualTo(model.DepartureId));
            Assert.That(2, Is.EqualTo(model.ArrivalId));
            Assert.That(dateTmr.AddHours(3), Is.EqualTo(model.ArrivalDate));
            Assert.That(dateTmr.AddHours(1), Is.EqualTo(model.DepartureDate));
            Assert.That(2, Is.EqualTo(model.CompanyId));
            Assert.That(2, Is.EqualTo(model.AirplaneId));
            Assert.That(true, Is.EqualTo(model.Snack));
            Assert.That(false, Is.EqualTo(model.Food));
            Assert.That(false, Is.EqualTo(model.Luggage));
            Assert.That(125, Is.EqualTo(model.Duration));           
        }
        [Test]
        public async Task TestDetails()
        {
            var repo = new Repository(DbContext);
            flightService = new FlightService(repo);

            DateTime datePast = DateTime.Parse("2022-11-19 16:30:00");
            DateTime dateTmr = DateTime.Now.AddDays(1);

            await repo.AddRangeAsync(new List<Flight>
            {
                new Flight()
                    {
                            DepartureId = 1,
                            ArrivalId = 2,
                            ArrivalDate = dateTmr.AddHours(3),
                            DepartureDate = dateTmr.AddHours(1),
                            CompanyId = 2,
                            Price = 5678,
                            AirplaneId = 2,
                            Airplane = new Airplane
                            {
                                Id=60000,
                                Capacity=4,
                                ManufactureId=1,
                                Model="Test"
                            },
                            Snack = true,
                            Food = false,
                            Luggage = false,
                            Duration = 125
                    },
                });
            await repo.AddRangeAsync(new List<Ticket>
            {
                new Ticket()
                {
                    guid  = Guid.Parse("61956A05-70F7-455E-40BE-08DADEAEA730"),
                    FlightId = 1,
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
            });

            await repo.SaveChangesAsync();

            var model = await flightService.Details(1);
            Assert.That(3, Is.EqualTo(model.AvailablePlaces));
        }

        [TearDown]
        public void TearDown()
        {
            DbContext.Dispose();
        }
    }
}


//Exception ex = Assert.ThrowsAsync<ArgumentException>(async () => await flightService.GetFlightById(4));
