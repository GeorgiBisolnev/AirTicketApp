using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Data.EntityModels.IdentityModels;
using AirTicketApp.Models.Ticket;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AirTicketApp.Services
{
    public class TicketService : ITicketService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository repo;
        private readonly IApplicationUserService userService;
        private readonly IFlightService flightService;

        public TicketService(UserManager<ApplicationUser> _userManager, 
            IRepository _repo,
            IApplicationUserService _userService,
            IFlightService _flightService)
        {
            this.userManager = _userManager;
            this.repo = _repo;
            this.userService = _userService;
            this.flightService = _flightService;
        }

        public async Task<List<TicketAllViewModel>> AllTicketsByUser(string userId)
        {

            var result = await repo.AllReadonly<Ticket>()
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(t => new TicketAllViewModel()
                {
                    TicketId = t.guid.ToString(),
                    FlightId = t.FlightId
                })
                .ToListAsync();

            foreach (var ticket in result)
            {
                var flightModel = await flightService.GetFlightById(ticket.FlightId);
                ticket.ArrivalDate = flightModel.ArrivalDate;
                ticket.DepartureDate = flightModel.DepartureDate;
                ticket.ArrivalAirportName = flightModel.ArrivalAirport.Name;
                ticket.ArrivalAirportCode = flightModel.ArrivalAirport.IATACode;
                ticket.ArrivalCity = flightModel.ArrivalAirport.City.Name;
                ticket.ArrivalCountry= flightModel.ArrivalAirport.City.Country.Name;
                ticket.DepartureAirportName = flightModel.DepartureAirport.Name;
                ticket.DepartureAirportCode = flightModel.DepartureAirport.IATACode;
                ticket.DepartureCity = flightModel.DepartureAirport.City.Name;
                ticket.DepartureCountry = flightModel.DepartureAirport.City.Country.Name;
                ticket.Price = flightModel.Price;
                ticket.CompanyName = flightModel.Company.Name;
            }

            return result.OrderByDescending(t => t.ArrivalDate).ToList();
        }

        public async Task<bool> AvailableTickets(int flightId, int capacity)
        {
            var result = await repo.AllReadonly<Ticket>()
                .AsNoTracking()
                .CountAsync(t=>t.FlightId==flightId);

            if (result< capacity)
            {
                return true;
            }

            return false;
        }

        public async Task<string> BuyTicket(int flightId, string userId, int capacity)
        {
            bool flightExsists = await flightService.FlightExists(flightId);
            Ticket ticket = new Ticket();
            var flight = await flightService.GetFlightById(flightId);
            DateTime departureDate = flight.DepartureDate;

            if (departureDate<DateTime.Now)
            {
                throw new ArgumentException("Departure date is in the past!");
            }
                
            var ticketsAvalibale = await AvailableTickets(flightId, capacity);
            if (flightExsists && ticketsAvalibale)
            {
                ticket.FlightId = flightId;
                ticket.UserId = userId;

                await repo.AddAsync(ticket);
                await repo.SaveChangesAsync();

                return ticket.guid.ToString();
            }
            else
            {
                throw new ArgumentException("There is no avalible tickets for this flight!");
            }

        }

        public async Task<string> MostPopularAirport()
        {
            var list = await repo.AllReadonly<Ticket>()
                .Include(t => t.Flight.ArrivalAirport)
                .GroupBy(t => t.Flight.ArrivalAirport.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(t=>t.Count)
                .ToListAsync();

            string result = list.FirstOrDefault().Name;

            if (result==null)
            {
                return "";
            }

            return result;
        }

        public async Task<int> TotalTickets()
        {
            return await repo.AllReadonly<Ticket>()
                .AsNoTracking()
                .CountAsync();
        }
    }
}
