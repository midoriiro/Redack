using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Redack.ServiceLayer.Models.Request.Uri
{
	public class DictionaryParameter : QueryParameter<Dictionary<object, object>>
	{
		public DictionaryParameter(QueryBuilder builder, HttpRequestMessage request) : base(builder, request)
		{
		}

		public DictionaryParameter(Dictionary<object, object> value) : base(value)
		{
		}

		public override IQueryable Execute(string key, IQueryable queryable)
		{
			throw new NotImplementedException();
		}

		public override void FromQuery(string value)
		{
			this.Value = JsonConvert.DeserializeObject<Dictionary<object, object>>(value);
		}

		public override string ToQuery()
		{
			return JsonConvert.SerializeObject(this.Value);
		}
	}
}