using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    [Table("Groups")]
    public class Group : Entity
    {
        [Required(ErrorMessage = "The name field is required")]
        [MinLength(3, ErrorMessage = "Type at least 3 characters")]
        public string Name { get; set; }

        // Navigation properties
        public virtual ICollection<User> Users { get; set; }
	    public virtual ICollection<Permission> Permissions { get; set; }
	}
}
