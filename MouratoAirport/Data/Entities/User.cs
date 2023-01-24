using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MouratoAirport.Data.Entities
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override string Email { get; set; }

        public string Password { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
