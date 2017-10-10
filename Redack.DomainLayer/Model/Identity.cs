using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    [Table("Identities")]
    public class Identity : Entity
    {
        [Required(ErrorMessage = "The access token field is required")]
        public string Access { get; set; }

        [Required(ErrorMessage = "The refresh token field is required")]
        public string Refresh { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The api key field is required")]
        public virtual ApiKey ApiKey { get; set; }

        [Required(ErrorMessage = "The client field is required")]
        public Client Client { get; set; }
    }
}