using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Redack.DomainLayer.Model;

namespace Redack.ServiceLayer.Models
{
    public class CredentialSignInRequest : Model
    {
        [Required]
        public Client Client { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}