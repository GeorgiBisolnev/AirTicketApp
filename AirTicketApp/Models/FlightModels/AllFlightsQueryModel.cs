using AirTicketApp.Models.AirportModels;
using System.ComponentModel.DataAnnotations;

namespace AirTicketApp.Models.FlightModels
{
    public class AllFlightsQueryModel
    {
        public int page { get; set; } = 1;

        [Required]
        public int ArrivalAirportId { get; set; }
        [Required]
        public int DepartureAirportId { get; set; }
        [Required]
        public DateTime SearchDate { get; set; }
        public FlightSorting? Sorting { get; set; }
        public IEnumerable<FlightViewModel>? Flights { get; set; } = Enumerable.Empty<FlightViewModel>();
        public IEnumerable<AirportViewModel> Airports { get; set; } = new List<AirportViewModel>();
    }
}
