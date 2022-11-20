using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.AirportModelConstants;

namespace AirTicketApp.Models
{
    public class AirportViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(NameMaximimLength)]
        public string Name { get; set; } = null!;

        public CityViewModel City { get; set; } = null!;

        [Required]
        public int CityId { get; set; }

        [Required]
        [StringLength(IATAexactlength)]
        public string IATACode { get; set; } = null!;
    }
}
