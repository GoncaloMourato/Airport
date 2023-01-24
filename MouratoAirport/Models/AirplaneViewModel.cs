using Microsoft.AspNetCore.Http;
using MouratoAirport.Data.Entities;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MouratoAirport.Models
{
    public class AirplaneViewModel : Airplane
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public int IdAirport { get; set; }

        public string NameAirport { get; set; }

        public string CityAirport { get; set; }

        public string LocationAirport { get; set; }

        public string TypeEdit { get; set; }
    }
}
