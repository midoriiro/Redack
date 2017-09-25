using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    [Table("Users")]
    public class User : Entity
    {
        [Required(ErrorMessage = "The alias field is required")]
        [MinLength(3, ErrorMessage = "Type at least 3 characters")]
        public string Alias { get; set; }

        public string IdentIcon { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The credential field is required")]
        public virtual Credential Credential { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
