using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    [Table("Permissions")]
    public class Permission : Entity
    {
        [Required(ErrorMessage = "The codename field is required")]
        public string Codename { get; set; }

        [Required(ErrorMessage = "The help text field is required")]
        [MinLength(5, ErrorMessage = "Type at least 5 characters")]
        public string HelpText { get; set; }

        [Required(ErrorMessage = "The content type field is required")]
        public string ContentType { get; set; }

        private Permission() {}

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
            return this.ContentType + "." + this.Codename;
        }
    }

}
