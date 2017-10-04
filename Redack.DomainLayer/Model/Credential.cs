using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Redack.DomainLayer.Exception;

namespace Redack.DomainLayer.Model
{
    [Table("Credentials")]
    public class Credential : Entity
    {
        [Required(ErrorMessage = "The login field is required")]
        [MaxLength(50, ErrorMessage = "Type less than 50 characters")]
        [Index(IsUnique = true)]
        [EmailAddress(ErrorMessage = "The login field is not a valid email address")]
        public string Login { get; set; }

        [Required(ErrorMessage = "The password field is required")]
        [MinLength(8, ErrorMessage = "Type at least 8 characters to ensure password security")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "The password confirmation field is required")]
        [Compare("Password", ErrorMessage = "Password confirmation does not match")]
        public string PasswordConfirm { get; set; }

        // Navigation properties
        public virtual ICollection<Credential> OldCredentials { get; set; }
    }
}
