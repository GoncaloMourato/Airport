using MouratoAirport.Data.Entities;
using System;
using System.Security.Principal;

namespace MouratoAirport.Data.Entities
{
    public class Flights : IEntity
    {
        public int Id { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public DateTime Date { get; set; }

        public Airplane Airplane { get; set; }

        public int AirplaneId { get; set; }

    }
}
