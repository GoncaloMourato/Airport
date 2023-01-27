using Microsoft.AspNetCore.Http;
using MouratoAirport.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MouratoAirport.Models
{
    public class EditProfileViewModel : User
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
