using System.ComponentModel.DataAnnotations;

namespace Redack.ServiceLayer.Models
{
    public class SignUpRequest : SignInRequest
    {
        [Required]
        public string PasswordConfirm { get; set; }
    }
}