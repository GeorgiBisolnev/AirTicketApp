using AirTicketApp.Data.EntityModels;

namespace AirTicketApp.Models
{
    public class AirplaneViewModel
    {
        public string Model { get; set; }

        public Manufacture Manufacture { get; set; }

        public int Capacity { get; set; }
    }
}
