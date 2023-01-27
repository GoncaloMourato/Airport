using MouratoAirport.Data.Entities;
using System.Collections.Generic;

namespace MouratoAirport.Models
{
    public class HistoryTicketViewModel
    {
        public IList<Ticket> Tickets { get; set; }

        public IList<Ticket> TicketsExpired { get; set; }
    }
}
