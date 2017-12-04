using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Redack.ConsumeLayer
{
	public class RestPromise
	{
		private enum ActionType
		{
			Done,
			Failed,
			Always
		}

		private List<KeyValuePair<ActionType, Action<HttpResponseMessage>>> Actions { get; set; }
		public RestRequest Request { get; protected set; }
		private HttpResponseMessage _response;

		public RestPromise(RestRequest request)
		{
			this.Actions = new List<KeyValuePair<ActionType, Action<HttpResponseMessage>>>();
			this.Request = request;
			this._response = null;
		}

		public HttpResponseMessage Execute(HttpResponseMessage response)
		{
			if(this._response == null)
				this._response = response;

			foreach(var action in this.Actions)
			{
				if (action.Key == ActionType.Done && response.IsSuccessStatusCode)
					action.Value(response);
				else if (action.Key == ActionType.Failed && !response.IsSuccessStatusCode)
					action.Value(response);
				else
					action.Value(response);
			}

			return this._response;
		}

		public RestPromise Done(Action<HttpResponseMessage> action)
		{
			this.Actions.Add(
				new KeyValuePair<ActionType, Action<HttpResponseMessage>>(
					ActionType.Done, 
					action));

			return this;
		}

		public RestPromise Failed(Action<HttpResponseMessage> action)
		{
			this.Actions.Add(
				new KeyValuePair<ActionType, Action<HttpResponseMessage>>(
					ActionType.Failed,
					action));

			return this;
		}

		public RestPromise Always(Action<HttpResponseMessage> action)
		{
			this.Actions.Add(
				new KeyValuePair<ActionType, Action<HttpResponseMessage>>(
					ActionType.Always,
					action));

			return this;
		}
	}
}
