using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirTicketApp.Data.EntityModels
{
    public class Airplane
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ManufactureId { get; set; }

        [ForeignKey(nameof(ManufactureId))]
        public Manufacture Manufacture { get; set; } = null!;

        [Required]
        public int Capacity { get; set; }

        [Required]
        public string Model { get; set; } = null!;

    }
}
