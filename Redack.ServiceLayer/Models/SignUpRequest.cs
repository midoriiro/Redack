using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Redack.ServiceLayer.Models
{
    public class SignUpRequest : SignInRequest
    {
        [Required]
        public string PasswordConfirm { get; set; }
    }
}