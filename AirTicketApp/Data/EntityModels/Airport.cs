using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirTicketApp.Data.EntityModels
{
    public class Airport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(CityId))]
        public City City { get; set; } = null!;

        [Required]
        public int CityId { get; set; }

        [Required]
        public string IATACode { get; set; } = null!;

    }
}
