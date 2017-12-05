using System.Linq;
using System.Net.Http;

namespace Redack.BridgeLayer.Messages.Uri
{
	public abstract class QueryParameter<T> : IQueryParameter
	{
		public bool IsUnique { get; protected set; } = true;

		public T Value { get; protected set; }
		public QueryBuilder Builder { get; set; }
		public HttpRequestMessage Request { get; set; }

		protected QueryParameter(QueryBuilder builder, HttpRequestMessage request)
		{
			this.Builder = builder;
			this.Request = request;
		}

		protected QueryParameter(T value)
		{
			this.Value = value;
		}

		public abstract IQueryable Execute(string key, IQueryable queryable);

		public abstract void FromQuery(string value);
		public abstract string ToQuery();
	}
}