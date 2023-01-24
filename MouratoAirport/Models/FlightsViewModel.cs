using MouratoAirport.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MouratoAirport.Models
{
    public class FlightsViewModel : Flights
    {
        public int AirplaneId { get; set; }

        public string Airplaneedit { get; set; }

        public IEnumerable<SelectListItem> Airplanes { get; set; }

    }
}
