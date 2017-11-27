using Redack.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace Redack.ServiceLayer.Models.Request.Uri
{
	public abstract class QueryParameter<T> : IQueryParameter
	{
		public T Value { get; set; }
		public QueryBuilder Builder { get; set; }
		public HttpRequestMessage Request { get; set; }

		public QueryParameter(QueryBuilder builder, HttpRequestMessage request)
		{
			this.Builder = builder;
			this.Request = request;
		}

		public QueryParameter(T value)
		{
			this.Value = value;
		}

		public abstract IQueryable Execute(string key, IQueryable queryable);

		public abstract void FromQuery(string value);
		public abstract string ToQuery();
	}
}