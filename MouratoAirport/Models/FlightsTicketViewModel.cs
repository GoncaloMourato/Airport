using MouratoAirport.Data.Entities;
using System.Collections.Generic;

namespace MouratoAirport.Models
{
    public class FlightsTicketViewModel : Flights
    {
        public IList<Flights> Flights { get; set; }
    }
}
