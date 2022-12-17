using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models.AirplaneModels;
using AirTicketApp.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AirTicketApp.Services
{
    public class AirplaneService : IAirplaneService
    {
        private readonly IRepository repo;

        public AirplaneService(IRepository _repo)
        {
            this.repo = _repo;
        }
        /// <summary>
        /// Процедурата прочита всички самолети в БД и ги връща като списък
        /// </summary>
        /// <returns>Връща списъчен модел на самолети</returns>
        public async Task<IEnumerable<AirplaneViewModel>> GetAllAirplanes()
        {
            var airplanes = await repo.AllReadonly<Airplane>()
                .Include(m => m.Manufacture)
                .Select(a => new AirplaneViewModel()
                {
                    Model = a.Model,
                    Id = a.Id,
                    Manufacture = a.Manufacture,
                    Capacity = a.Capacity,
                })
                .ToListAsync();

            return airplanes;
        }
    }
}
