using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using AirTicketApp.Models.FlightModels;

namespace AirTicketApp.Services
{
    public class FlightService : IFlightService
    {
        private readonly IRepository repo;

        public FlightService(IRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task<IEnumerable<FlightViewModel>> AllFlights()
        {
            var flights =  await repo.AllReadonly<Flight>()
                .Include(m=>m.Airplane.Manufacture)
                .Include(c=>c.ArrivalAirport.City)
                .Where(d=>d.DepartureDate>=DateTime.Today)
                .Select(f=> new FlightViewModel()
                {
                    Id=f.Id,
                    ArrivalAirport = f.ArrivalAirport,
                    DepartureAirport = f.DepartureAirport,
                    ArrivalDate = f.ArrivalDate,
                    DepartureDate = f.DepartureDate,
                    Duration = f.Duration,
                    Company = f.Company,
                    Airplane = f.Airplane,
                    Price = f.Price,
                    Snack = f.Snack,
                    Food = f.Food,
                    Luggage = f.Luggage,
                })
                .ToListAsync();
            
            return flights;
        }

        public async Task<int> Create(FlightViewModel model)
        {
            var flight = new Flight()
            {
                ArrivalId = model.ArrivalId,
                DepartureId = model.DepartureId,
                ArrivalDate = model.ArrivalDate,
                DepartureDate = model.DepartureDate,
                CompanyId = model.CompanyId,
                Price = model.Price,
                AirplaneId = model.AirplaneId,
                Snack = model.Snack,
                Food = model.Food,
                Luggage = model.Luggage,
                Duration = model.Duration
            };

            await repo.AddAsync(flight);
            await repo.SaveChangesAsync();

            return flight.Id;
        }
        /// <summary>
        /// finds detiled information about flights
        /// </summary>
        /// <param name="flightId"></param>
        /// <returns>Flight model</returns>
        /// <exception cref="Exception"></exception>
        public async Task<FlightViewModelDetails> Details(int Id)
        {

            var f = await repo.AllReadonly<Flight>()
                            .Include(m => m.Airplane.Manufacture)
                            .Include(c => c.ArrivalAirport.City.Country)
                            .Include(c => c.DepartureAirport.City.Country)
                            .Include(c=>c.Company)
                            .FirstOrDefaultAsync(f => f.Id == Id);

            int buyedFlights = await repo.AllReadonly<Ticket>()
                .CountAsync(t=>t.FlightId==Id);

            if (f==null)
            {
                throw new Exception ("We cant find flight information!");
            }

            
            var flightModel = new FlightViewModelDetails()
            {
                Id = f.Id,
                ArrivalAirport = f.ArrivalAirport,
                DepartureAirport = f.DepartureAirport,
                ArrivalDate = f.ArrivalDate,
                DepartureDate = f.DepartureDate,
                Duration = f.Duration,
                Company = f.Company,
                Airplane = f.Airplane,
                Price = f.Price,
                Snack = f.Snack,
                Food = f.Food,
                Luggage = f.Luggage,
                AvailablePlaces = f.Airplane.Capacity - buyedFlights,
            };

            return flightModel;
        }

        public async Task<IEnumerable<FlightViewModel>> AllFlightsFilter(
            FlightSorting? sorting = 0,
            DateTime? searchDate = null,
            int? ArrivalAirportId = null,
            int? DepartureAirportId = null)
        {
            if (searchDate==null || ArrivalAirportId == null || DepartureAirportId == null ||
                ArrivalAirportId == 0 || DepartureAirportId==0)
            {
                throw new ArgumentException("Wring input parameters"); 
            }
            var result = await AllFlights();

            result = result.Where(f => f.ArrivalAirport.Id == ArrivalAirportId);

            result = result.Where(f => f.DepartureAirport.Id == DepartureAirportId);

            result = result.Where(f => f.DepartureDate.Date == searchDate);

            result = sorting switch
            {
                FlightSorting.Price => result
                    .OrderBy(f=>f.Price).ToList(),
                FlightSorting.Company=>result
                    .OrderByDescending(f=>f.Company.Name).ToList(),
                FlightSorting.DepartureDate => result
                    .OrderBy(f=>f.DepartureDate).ToList(),
                FlightSorting.ArrivalDate=> result
                    .OrderBy(f => f.ArrivalDate).ToList(),
                _ => result.OrderBy(f=>f.Id).ToList()
            };

            return result;
        }

        public async Task Edit(int flightId, FlightViewModel model)
        {
            var flight = await repo.All<Flight>()
                .Include(f=>f.ArrivalAirport)
                .Include(f=>f.DepartureAirport)
                .Include(c=>c.Company)
                .FirstOrDefaultAsync(f=>f.Id==flightId);

            if (flight == null)
            {
                throw new ArgumentNullException("There is no flightwith such id");
            }

            flight.ArrivalId = model.ArrivalId;
            flight.DepartureId = model.DepartureId;
            flight.ArrivalDate = model.ArrivalDate;
            flight.DepartureDate = model.DepartureDate;
            if (await BuyedFlightsByGivenFlightId(flightId)==false)
            {
                flight.CompanyId = model.CompanyId;
                flight.AirplaneId = model.AirplaneId;
            } else
            {
                throw new ArgumentException("Company and Airplane cant be chaged due to avalible tickets for this flight!");
            }            
            flight.Snack = model.Snack;
            flight.Food = model.Food;
            flight.Luggage = model.Luggage;
            flight.Price = model.Price;
            flight.Duration = model.Duration;

            await repo.SaveChangesAsync();
        }

        public async Task<bool> BuyedFlightsByGivenFlightId(int flightId)
        {
            var result = await repo.AllReadonly<Ticket>()
                .FirstOrDefaultAsync(t => t.FlightId == flightId);

            if (result==null)
            {
                return false;
            }

            return true;
        }
        public async Task<FlightViewModel> GetFlightById(int Id)
        {
            var model = await repo.AllReadonly<Flight>()
                .Where(f=>f.Id==Id)
                .Include(f=>f.ArrivalAirport)
                .Include(f=>f.DepartureAirport)
                .Include(c=>c.Company)
                .Select(f => new FlightViewModel()
                {
                    Id = f.Id,
                    ArrivalAirport = f.ArrivalAirport,
                    DepartureAirport = f.DepartureAirport,
                    ArrivalDate = f.ArrivalDate,
                    DepartureDate = f.DepartureDate,
                    Duration = f.Duration,
                    Company = f.Company,
                    CompanyId=f.CompanyId,
                    Airplane = f.Airplane,
                    Price = f.Price,
                    Snack = f.Snack,
                    Food = f.Food,
                    Luggage = f.Luggage,
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                throw new ArgumentNullException("There is no flight with such Id!");
                return new FlightViewModel();
            }

            return model;
        }
    }
}
