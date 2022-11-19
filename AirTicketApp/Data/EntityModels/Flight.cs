using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AirTicketApp.Data.Common.FlightModelConstants;

namespace AirTicketApp.Data.EntityModels
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DepartureId { get; set; }

        [ForeignKey(nameof(DepartureId))]
        public Airport DepartureAirport { get; set; } = null!;

        [Required]
        public int ArrivalId { get; set; }

        [ForeignKey(nameof(ArrivalId))]
        public Airport ArrivalAirport { get; set; } = null!;

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        public DateTime ArrivalDate { get; set; }

        [MaxLength(MaxFlightDuration)]
        public int? Duration { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        [Required]
        [Column(TypeName = "money")]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        [Required]
        public int AirplaneId { get; set; }

        [ForeignKey(nameof(AirplaneId))]
        public Airplane Airplane { get; set; } = null!;

        public bool? Drinks { get; set; }

        public bool? Food { get; set; }

        public bool? Snack { get; set; }

        public bool? Luggage { get; set; }

    }
}
