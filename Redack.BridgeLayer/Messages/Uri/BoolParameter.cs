using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Redack.BridgeLayer.Messages.Uri
{
	public class BoolParameter : QueryParameter<bool>
	{
		public BoolParameter(QueryBuilder builder, HttpRequestMessage request) : base(builder, request)
		{
			this.IsUnique = false;
		}

		public BoolParameter(bool value) : base(value)
		{
		}

		public override IQueryable Execute(string key, IQueryable queryable)
		{
			throw new NotImplementedException();
		}

		public override void FromQuery(string value)
		{
			this.Value = JsonConvert.DeserializeObject<bool>(value);
		}

		public override string ToQuery()
		{
			return JsonConvert.SerializeObject(this.Value);
		}
	}
}