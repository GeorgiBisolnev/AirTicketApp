using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Services.Contracts;
using AirTicketApp.Models;
using Microsoft.EntityFrameworkCore;
using AirTicketApp.Models.Flight;

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
            var result = await AllFlights();

            if (ArrivalAirportId != null)
            {
                result = result.Where(f => f.ArrivalAirport.Id == ArrivalAirportId);
            }

            if (DepartureAirportId != null)
            {
                result = result.Where(f => f.DepartureAirport.Id == DepartureAirportId);
            }
            if (searchDate != null)
            {
                result = result.Where(f => f.DepartureDate.Date == searchDate);
            }

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
    }
}
