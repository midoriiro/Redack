using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
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
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Group> Groups { get; set; }

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
