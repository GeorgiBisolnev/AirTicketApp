using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static AirTicketApp.Data.Common.CityModelConstants;

namespace AirTicketApp.Data.EntityModels
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxCityNameLength)]
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; } = null!;

        [Required]
        public int CountryId { get; set; }
    }
}
