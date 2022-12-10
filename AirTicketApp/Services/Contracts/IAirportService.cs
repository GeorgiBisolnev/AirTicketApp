using AirTicketApp.Models.AirportModels;

namespace AirTicketApp.Services.Contracts
{
    public interface IAirportService
    {
        Task<IEnumerable<AirportViewModel>> GetAllAirports();
    }
}
