using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models;

namespace AirTicketApp.Services.Contracts
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightViewModel>> AllFlights(); 
    }
}
