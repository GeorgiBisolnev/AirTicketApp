using AirTicketApp.Data.EntityModels;
using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.CityModelConstants;
namespace AirTicketApp.Models
{
    public class CityViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(MaxCityNameLength)]
        public string Name { get; set; } = null!;

        public Country Country { get; set; } = null!;

        [Required]
        public int CountryId { get; set; }
    }
}
