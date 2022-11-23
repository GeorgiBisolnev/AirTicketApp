using AirTicketApp.Data.EntityModels;
using AirTicketApp.Models.ValidationClassAtributes;
using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.FlightModelConstants;

namespace AirTicketApp.Models
{
    public class FlightViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [NotEqualTo("ArrivalId",ErrorMessage ="Departure airport can't be the same as arrival airport")]
        public int DepartureId { get; set; }    

        public Airport? DepartureAirport { get; set; } = null!;

        [Required]
        public int ArrivalId { get; set; }

        public Airport? ArrivalAirport { get; set; } = null!;

        [Required]
        [DateLessThan("ArrivalDate", ErrorMessage ="Departure date and time must be less than Arrival date")]
        public DateTime DepartureDate { get; set; }

        [Required]
        public DateTime ArrivalDate { get; set; }

        [Range(1,MaxFlightDuration)]
        public int? Duration { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company? Company { get; set; } = null!;

        [Required]
        [Display(Name = "Price of the flight")]
        [Range(1, 500000.00, ErrorMessage = "Price per must be a positive number and less than {2} leva")]
        public decimal Price { get; set; }

        [Required]
        public int AirplaneId { get; set; }

        public Airplane? Airplane { get; set; } = null!;

        public bool Food { get; set; }

        public bool Snack { get; set; }

        public bool Luggage { get; set; }

        public IEnumerable<AirportViewModel> Airports { get; set; } = new List<AirportViewModel>();
        public IEnumerable<CompanyViewModel> Companies { get; set; } = new List<CompanyViewModel>();
        public IEnumerable<AirplaneViewModel> Airplanes { get; set; } = new List<AirplaneViewModel>();

        public string?  DateTimeNowFormated { get; set; }

    }
}
