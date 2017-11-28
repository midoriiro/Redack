using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Redack.ConsumeLayer
{
	class RestPromise
	{
		private readonly Task<HttpResponseMessage> _promise;
		private HttpResponseMessage _response = null;

		public RestPromise(Task<HttpResponseMessage> promise)
		{
			this.Done((v) => { v.Content = null; });
		}

		private HttpResponseMessage GetResponse()
		{
			if (this._response == null)
				this._response = this._promise.Result;

			return this._response;
		}

		public RestPromise Done(Action<HttpResponseMessage> action)
		{
			var response = this.GetResponse();

			if(response.IsSuccessStatusCode)
				action(response);

			return this;
		}

		public RestPromise Failed(Action<HttpResponseMessage> action)
		{
			var response = this.GetResponse();

			if (!response.IsSuccessStatusCode)
				action(response);

			return this;
		}
	}
}
