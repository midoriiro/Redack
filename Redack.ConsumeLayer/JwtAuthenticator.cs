using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Redack.ConsumeLayer
{
	class JwtAuthenticator : IAuthenticator
	{
		private readonly string _token;

		public JwtAuthenticator(string token)
		{
			this._token = token;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			if (!request.Parameters.Any(
				e => e.Type.Equals(ParameterType.HttpHeader) &&
				e.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
			{
				request.AddParameter("Authorization", this?._token, ParameterType.HttpHeader);
			}				
		}
	}
}
