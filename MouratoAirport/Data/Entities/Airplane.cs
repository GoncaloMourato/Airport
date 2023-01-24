using System;
using System.Security.Principal;

namespace MouratoAirport.Data.Entities
{
    public class Airplane : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public int Seat { get; set; }

        public string ImageUrl { get; set; }

    }
}
