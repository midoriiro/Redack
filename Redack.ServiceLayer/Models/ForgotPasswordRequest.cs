using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DomainLayer.Model;

namespace Redack.ServiceLayer.Models
{
    public class ForgotPasswordRequest : Model
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