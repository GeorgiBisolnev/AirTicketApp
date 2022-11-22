using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models;

namespace AirTicketApp.Services.Contracts
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightViewModel>> AllFlights(); 
        Task<IEnumerable<AirportViewModel>> GetAllAirports(); 
        Task<IEnumerable<CompanyViewModel>> GetAllCompanies(); 
        Task<IEnumerable<AirplaneViewModel>> GetAllAirplanes(); 
        Task<int> Create(FlightViewModel model); 
    }
}
