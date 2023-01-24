using MouratoAirport.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using MouratoAirport.Data;
using System.Collections.Generic;
using System.Linq;

namespace MouratoAirport.Data
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {

        private readonly DataContext _context;

        public TicketRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboTickets()
        {
            var list = _context.Tickets.Select(p => new SelectListItem
            {
                Text = p.Flights.ToString(),
                Value = p.Id.ToString()
            }
            ).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Ticket ...)",
                Value = "0"
            });

            return list;
        }
    }
}
