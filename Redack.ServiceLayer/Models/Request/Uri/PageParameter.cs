using Newtonsoft.Json;
using Redack.DatabaseLayer.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Redack.ServiceLayer.Models.Request.Uri
{
	public class PageParameter : QueryParameter<Dictionary<string, int>>
	{
		private readonly int DefaultPageIndex = 1;
		private readonly int DefaultPageSize = 10;
		private readonly int MinPageSize = 1;
		private readonly int MaxPageSize = 100;

		public PageParameter(QueryBuilder builder, HttpRequestMessage request) : base(builder, request)
		{
		}

		public PageParameter(int index = 1, int? size = null) : base(null)
		{
			if (index < this.DefaultPageIndex)
				index = this.DefaultPageIndex;

			if (size == null)
				size = this.DefaultPageSize;
			else if (size > MaxPageSize)
				size = this.MaxPageSize;
			else if (size < MinPageSize)
				size = this.MinPageSize;

			this.Value = new Dictionary<string, int>()
			{
				{ "index", index },
				{ "size", (int)size },
			};
		}

		public void Normalize()
		{
			if (!this.Value.ContainsKey("index"))
				this.Value["index"] = this.DefaultPageIndex;
			else if (this.Value["index"] < this.DefaultPageIndex)
				this.Value["index"] = this.DefaultPageIndex;

			if (!this.Value.ContainsKey("size"))
				this.Value["size"] = this.DefaultPageSize;
			else if (this.Value["size"] > MaxPageSize)
				this.Value["size"] = this.MaxPageSize;
			else if (this.Value["size"] < MinPageSize)
				this.Value["size"] = this.MinPageSize;
		}

		public override IQueryable Execute(string key, IQueryable queryable)
		{
			var methodName = "Paginate";
			var method = typeof(QueryableExtensions).GetMethod(methodName);

			if (method == null)
				throw new MissingMethodException($"{methodName} does not exists in QueryableExtensions class");

			this.Normalize();

			return (IQueryable)method.Invoke(queryable, new object[]
			{
					queryable,
					this.Value["index"],
					this.Value["size"]
			});
		}

		public override void FromQuery(string value)
		{
			this.Value = JsonConvert.DeserializeObject<Dictionary<string, int>>(value);
		}

		public override string ToQuery()
		{
			return JsonConvert.SerializeObject(this.Value);
		}

		public static explicit operator PageParameter(QueryParameter<dynamic> v)
		{
			throw new NotImplementedException();
		}
	}
}