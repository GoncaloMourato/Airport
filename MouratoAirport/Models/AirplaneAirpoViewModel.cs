using MouratoAirport.Data.Entities;
using System.Collections;
using System.Collections.Generic;

namespace MouratoAirport.Models
{
    public class AirplaneAirpoViewModel
    {
        public IList<Airplane> Airplane { get; set; }

        public IList<Airpo> Airpo { get; set; }
    }
}
