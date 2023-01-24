using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MouratoAirport.Models
{
    public class AddTicketViewModel
    {
        [Display(Name = "Ticket")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a ticket.")]
        public int TicketId { get; set; }


        [Range(0.0001, int.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public int Quantity { get; set; }

        public IEnumerable<SelectListItem> Tickets { get; set; }
    }
}
