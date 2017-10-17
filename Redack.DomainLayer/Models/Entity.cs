using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

#pragma warning disable 659

namespace Redack.DomainLayer.Models
{
	public abstract class Entity : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		public override bool Equals(object obj)
		{
			if (obj == null || this.GetType() != obj.GetType())
				return false;

			PropertyInfo[] properties = this.GetType().GetProperties();

			foreach (var property in properties)
			{
				object value1 = this.GetType().GetProperty(property.Name)?.GetValue(this, null);
				object value2 = obj.GetType().GetProperty(property.Name)?.GetValue(obj, null);

				if (value1 != value2)
					return false;
			}

			return true;
		}

		public abstract void Update();
		public abstract void Delete();
	}
}
