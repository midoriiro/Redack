using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
    public class Identity : Entity
    {
        [Required(ErrorMessage = "The token field is required")]
        public string Token { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "The api key field is required")]
        public virtual ApiKey ApiKey { get; set; }
    }
}