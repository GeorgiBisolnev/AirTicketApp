using AirTicketApp.Data.Common.Repository;
using AirTicketApp.Data.EntityModels;
using AirTicketApp.Data.EntityModels.IdentityModels;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;

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
            //var ticketsAvalibale = await AvailableTickets(flightId, capacity);
            if (flightExsists && true)
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


    }
}
