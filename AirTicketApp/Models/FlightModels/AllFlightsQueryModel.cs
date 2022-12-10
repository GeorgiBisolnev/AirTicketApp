using AirTicketApp.Models.AirportModels;

namespace AirTicketApp.Models.FlightModels
{
    public class AllFlightsQueryModel
    {
        public int page { get; set; } = 1;
        public int? ArrivalAirportId { get; set; }
        public int? DepartureAirportId { get; set; }
        public DateTime? SearchDate { get; set; }
        public FlightSorting? Sorting { get; set; }
        public IEnumerable<FlightViewModel>? Flights { get; set; } = Enumerable.Empty<FlightViewModel>();
        public IEnumerable<AirportViewModel> Airports { get; set; } = new List<AirportViewModel>();
    }
}
