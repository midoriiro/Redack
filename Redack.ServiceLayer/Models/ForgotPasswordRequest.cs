using Redack.DomainLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace Redack.ServiceLayer.Models
{
    public class ForgotPasswordRequest : BaseModel
    {
        [Required]
        public Client Client { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string NewPasswordConfirm { get; set; }
    }
}