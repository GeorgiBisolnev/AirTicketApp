using AirTicketApp.Models.FlightModels;

namespace AirTicketApp.Services.Contracts
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightViewModel>> AllFlights(); 
        Task<IEnumerable<FlightViewModel>> AllFlightsFilter(
            FlightSorting? sorting = null,
            DateTime? searchDate = null,
            int? ArrivalAirportId = null,
            int? DepartureAirportId = null
            ); 
        Task<FlightViewModelDetails> Details(int id); 
        Task<int> Create(FlightViewModel model);
        Task Edit(int flightId, FlightViewModel model);
        Task<bool> BuyedFlightsByGivenFlightId(int flightId);
        Task<bool> FlightExists(int flightId);
        Task<FlightViewModel> GetFlightById(int Id);
        Task<IEnumerable<FlightViewCarouselModel>> GetMostCheapThreeFlights();
        Task<string> MostExpensiveDestination();
    }
}
