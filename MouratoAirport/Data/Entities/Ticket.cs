using Microsoft.AspNetCore.Mvc.ModelBinding;
using MouratoAirport.Data.Entities;
using System;

namespace MouratoAirport.Data.Entities
{
    public class Ticket : IEntity
    {
        public User User { get; set; } 

        public string UserId { get; set; }

        public int Id { get; set; }

        public string Number { get; set; }

        public int FlightsId { get; set; }

        public Flights Flights { get; set; }

        public int Price { get; set; }

        public string Seat { get; set; }

        public string TypeSeat { get; set; }

        public string Name { get; set; }

        public string NameOnCard { get; set; }

        public string CardNumber { get; set; }

        public string Date { get; set; }

        public string ExpiredDate { get; set; }

        public int Cvv { get; set; }
    }
}
