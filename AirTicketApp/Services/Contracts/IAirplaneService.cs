using AirTicketApp.Models.AirplaneModels;

namespace AirTicketApp.Services.Contracts
{
    public interface IAirplaneService
    {
        Task<IEnumerable<AirplaneViewModel>> GetAllAirplanes();
    }
}
