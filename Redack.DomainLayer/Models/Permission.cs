using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Redack.DomainLayer.Models
{
    [Table("Permissions")]
    public class Permission : Entity
    {
        [Required(ErrorMessage = "The codename field is required")]
        [Index("UIX_ContentTypeAndCodename", IsUnique = true, Order = 2)]
        public string Codename { get; set; }

        [Required(ErrorMessage = "The help text field is required")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        public string HelpText { get; set; }

        [Required(ErrorMessage = "The content type field is required")]
        [Index("UIX_ContentTypeAndCodename", IsUnique = true, Order = 1)]
        public string ContentType { get; set; }

        // Navigation properties
        public virtual IList<User> Users { get; set; } = new List<User>();
        public virtual IList<Group> Groups { get; set; } = new List<Group>();

        public static Permission Create<T>(string codename, string helpText) where T : Entity
        {
            return new Permission()
            {
                Codename = codename,
                HelpText = helpText,
                ContentType = typeof(T).Name
            };
        }

        public override string ToString()
        {
            return $"{this.ContentType}.{this.Codename}";
        }

        public override IQueryable<Entity> Filter(IQueryable<Entity> query)
        {
            throw new NotImplementedException();
        }

        public override List<Entity> Delete()
        {
            foreach (var user in this.Users)
            {
                for (int i = 0; i < user.Permissions.Count; i++)
                {
                    var permission = user.Permissions.ElementAt(i);

                    if (permission.Id == this.Id)
                        user.Permissions.RemoveAt(i);
                }
            }

            this.Users.Clear();

            foreach (var group in this.Groups)
            {
                for (int i = 0; i < group.Permissions.Count; i++)
                {
                    var permission = group.Permissions.ElementAt(i);

                    if (permission.Id == this.Id)
                        group.Permissions.RemoveAt(i);
                }
            }

            this.Groups.Clear();

            return null;
        }
    }

}
