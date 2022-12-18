using AirTicketApp.Models.AirportModels;
using AirTicketApp.Models.ValidationClassAtributes;
using System.ComponentModel.DataAnnotations;

namespace AirTicketApp.Models.FlightModels
{
    public class AllFlightsQueryModel
    {
        public int page { get; set; } = 1;

        [Required(ErrorMessage = "Departure airport is Required")]
        [NotEqualTo("DepartureAirportId", ErrorMessage = "Departure airport can't be the same as arrival airport")]
        public int ArrivalAirportId { get; set; }
        [Required(ErrorMessage = "Arrival airport is Required")]
        [NotEqualTo("ArrivalAirportId", ErrorMessage = "Departure airport can't be the same as arrival airport")]
        public int DepartureAirportId { get; set; }
        [Required(ErrorMessage = "Departure airport is Required")]        
        public DateTime SearchDate { get; set; }
        public FlightSorting? Sorting { get; set; }
        public IEnumerable<FlightViewModel>? Flights { get; set; } = Enumerable.Empty<FlightViewModel>();
        public IEnumerable<AirportViewModel> Airports { get; set; } = new List<AirportViewModel>();
    }
}
