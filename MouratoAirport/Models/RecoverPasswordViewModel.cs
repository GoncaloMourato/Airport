using System.ComponentModel.DataAnnotations;

namespace MouratoAirport.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
