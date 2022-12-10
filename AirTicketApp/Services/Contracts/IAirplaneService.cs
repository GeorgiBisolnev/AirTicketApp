using AirTicketApp.Models;

namespace AirTicketApp.Services.Contracts
{
    public interface IAirplaneService
    {
        Task<IEnumerable<AirplaneViewModel>> GetAllAirplanes();
    }
}
