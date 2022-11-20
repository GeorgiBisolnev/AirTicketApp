using AirTicketApp.Data.EntityModels;
using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.FlightModelConstants;

namespace AirTicketApp.Models
{
    public class FlightViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DepartureId { get; set; }    

        public Airport DepartureAirport { get; set; } = null!;

        [Required]
        public int ArrivalId { get; set; }

        public Airport ArrivalAirport { get; set; } = null!;

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        public DateTime ArrivalDate { get; set; }

        [MaxLength(MaxFlightDuration)]
        public int? Duration { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; } = null!;

        [Required]
        [Display(Name = "Price of the flight")]
        public decimal Price { get; set; }

        [Required]
        public int AirplaneId { get; set; }

        public Airplane Airplane { get; set; } = null!;

        public bool? Drinks { get; set; }

        public bool? Food { get; set; }

        public bool? Snack { get; set; }

        public bool? Luggage { get; set; }
    }
}
