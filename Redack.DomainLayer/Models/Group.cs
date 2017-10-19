using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Redack.DomainLayer.Models
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
        public virtual IList<User> Users { get; set; } = new List<User>();
        public virtual IList<Permission> Permissions { get; set; } = new List<Permission>();

        public override void Delete()
        {
            foreach (var user in this.Users)
            {
                for (int i = 0; i < user.Groups.Count; i++)
                {
                    var group = user.Groups.ElementAt(i);

                    if (group.Id == this.Id)
                        user.Groups.RemoveAt(i);
                }
            }

            this.Users.Clear();

            foreach (var permission in this.Permissions)
            {
                for (int i = 0; i < permission.Groups.Count; i++)
                {
                    var group = permission.Groups.ElementAt(i);

                    if(group.Id == this.Id)
                        permission.Groups.RemoveAt(i);
                }
            }

            this.Permissions.Clear();
        }
    }
}
