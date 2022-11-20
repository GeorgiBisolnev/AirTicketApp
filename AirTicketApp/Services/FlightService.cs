using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Services.Contracts;
using AirTicketApp.Models;
using Microsoft.EntityFrameworkCore;

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
                })
                .ToListAsync();

            return flights;
        }
    }
}
