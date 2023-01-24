using MouratoAirport.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using MouratoAirport.Data;
using System.Collections;
using System.Collections.Generic;

namespace MouratoAirport.Data
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        IEnumerable<SelectListItem> GetComboTickets();
    }
}
