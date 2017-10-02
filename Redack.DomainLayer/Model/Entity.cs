using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redack.DomainLayer.Model
{
	public abstract class Entity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public override bool Equals(object obj)
		{
			return obj != null && this.GetHashCode() == obj.GetHashCode();
		}

		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}
	}
}
