using AirTicketApp.Models;

namespace AirTicketApp.Services.Contracts
{
    public interface IAirportService
    {
        Task<IEnumerable<AirportViewModel>> GetAll();
    }
}
