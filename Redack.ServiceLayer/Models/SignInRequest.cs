using Redack.DomainLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace Redack.ServiceLayer.Models
{
    public class SignInRequest : Model
    {
        [Required]
        public Client Client { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}