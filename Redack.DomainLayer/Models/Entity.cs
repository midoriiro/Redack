using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

#pragma warning disable 659

namespace Redack.DomainLayer.Models
{
	public abstract class Entity : IEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None | DatabaseGeneratedOption.Identity)]
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

		[NotMapped]
		public bool CanBeDeleted { get; protected set; } = true;

		public abstract IQueryable<Entity> Filter(IQueryable<Entity> query);
		public abstract List<Entity> Delete();
	}
}
