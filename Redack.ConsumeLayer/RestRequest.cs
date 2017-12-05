using Redack.BridgeLayer.Messages.Request;
using Redack.BridgeLayer.Messages.Uri;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Redack.ConsumeLayer
{
	public class RestRequest
	{
		public string Resource { get; private set; }
		public string Query { get; private set; }
		public HttpRequestMessage Message { get; private set; }

		public RestRequest(HttpMethod method, string resource)
		{
			this.Resource = resource;
			this.Message = new HttpRequestMessage(method, "");
		}

		public void SetObjectContent<TFormatter>(IEntityRequest obj) where TFormatter : MediaTypeFormatter, new()
		{
			var formatter = new TFormatter();

			this.Message.Content = new ObjectContent(
				obj.GetType(), 
				obj, 
				formatter, 
				formatter.SupportedMediaTypes[0].MediaType);
		}

		public void SetContent(HttpContent content)
		{
			this.Message.Content = content;
		}

		public void SetAuthorization(string key, string parameter)
		{
			this.Message.Headers.Authorization = new AuthenticationHeaderValue(key, parameter);
		}

		public void SetQyery(string query)
		{
			this.Query = query;
		}

		public void SetQuery(QueryBuilder query)
		{
			this.Query = query.ToQueryString();
		}
	}
}
