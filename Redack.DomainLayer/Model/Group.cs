using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
	[Table("Groups")]
	public class Group : Entity
	{
		[Required(ErrorMessage = "The name field is required")]
		[MaxLength(15, ErrorMessage = "Type less than 15 characters")]
		[MinLength(3, ErrorMessage = "Type at least 3 characters")]
		[Index(IsUnique = true)]
		public string Name { get; set; }

		// Navigation properties
		public virtual ICollection<User> Users { get; set; }
		public virtual ICollection<Permission> Permissions { get; set; }

	    public override void Update()
	    {
	        throw new System.NotImplementedException();
	    }

	    public override void Delete()
	    {
	        throw new System.NotImplementedException();
	    }
	}
}
