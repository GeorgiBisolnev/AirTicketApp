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

        public async Task<IEnumerable<AirportViewModel>> GetAllAirports()
        {
            var airports = await repo.AllReadonly<Airport>()
                .Include(c => c.City.Country)
                .Select(a => new AirportViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    City = a.City,
                    IATACode = a.IATACode,
                })
                .ToListAsync();

            return airports;
        }

        public async Task<IEnumerable<CompanyViewModel>> GetAllCompanies()
        {
            var companies = await repo.AllReadonly<Company>()
                .Select(c => new CompanyViewModel()
                {
                    Name = c.Name,
                    Id = c.Id,


                })
                .ToListAsync();

            return companies;
        }

        public async Task<IEnumerable<AirplaneViewModel>> GetAllAirplanes()
        {
            var airplanes = await repo.AllReadonly<Airplane>()
                .Include(m => m.Manufacture)
                .Select(a => new AirplaneViewModel()
                {
                    Model = a.Model,
                    Id = a.Id,
                    Manufacture = a.Manufacture,
                })
                .ToListAsync();

            return airplanes;
        }
    }
}
