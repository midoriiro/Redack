using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Linq;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace Redack.ServiceLayer.Models.Request.Uri
{
	public sealed class QueryBuilderException : Exception
	{
		public QueryBuilderException(string message) : base(message)
		{
		}
	}
}