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
        [Required(ErrorMessage = "The user field is required")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "The client field is required")]
        public virtual Client Client { get; set; }

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