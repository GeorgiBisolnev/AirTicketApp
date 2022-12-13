namespace AirTicketApp.Models.Ticket
{
    public class TicketAllViewModel
    {
        public string TicketId { get; set; }
        public string ArrivalAirportName { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalCountry { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureAirportCode { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureCountry { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal Price { get; set; }
        public int FlightId { get; set; }
        public string UserId { get; set; }
    }
}
