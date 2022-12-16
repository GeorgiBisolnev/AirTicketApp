using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models.AirportModels;
using AirTicketApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirTicketApp.Services
{
    public class AirportService : IAirportService
    {
        private readonly IRepository repo;

        public AirportService(IRepository _repo)
        {
            this.repo = _repo;
        }
        /// <summary>
        /// Процедурата прочита всички летища в БД и ги връща като списък
        /// </summary>
        /// <returns>Процедурата прочита всички летища в БД и ги връща като списък</returns>
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
    }
}
