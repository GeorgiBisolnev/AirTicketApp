using AirTicketApp.Models.Ticket;

namespace AirTicketApp.Services.Contracts
{
    public interface ITicketService 
    {
        Task<string> BuyTicket(int flightId, string userId, int capacity);
        Task<bool> AvailableTickets(int flightId, int capacity);
        Task<List<TicketAllViewModel>> AllTicketsByUser(string userId);
    }
}
