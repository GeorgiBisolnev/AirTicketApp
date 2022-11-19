using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AirTicketApp.Data.Common.AirportModelConstants;

namespace AirTicketApp.Data.EntityModels
{
    public class Airport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaximimLength)]
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(CityId))]
        public City City { get; set; } = null!;

        [Required]
        public int CityId { get; set; }

        [Required]
        [StringLength(IATAexactlength)]
        public string IATACode { get; set; } = null!;

    }
}
