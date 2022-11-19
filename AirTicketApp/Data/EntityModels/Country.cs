using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.CountryModelConstants;

namespace AirTicketApp.Data.EntityModels
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxCountryNameLength)]
        public string Name { get; set; } = null!;
    }
}
