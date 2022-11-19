using System.ComponentModel.DataAnnotations;
using static AirTicketApp.Data.Common.ManufactureModelConstants;

namespace AirTicketApp.Data.EntityModels
{
    public class Manufacture
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(MaxManufactureNameLegth)]
        public string Name { get; set; } = null!;
    }
}
