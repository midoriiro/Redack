using Redack.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace Redack.ServiceLayer.Models.Request.Uri
{
	public interface IQueryParameter
	{
		QueryBuilder Builder { get; set; }
		HttpRequestMessage Request { get; set; }

		IQueryable Execute(string key, IQueryable queryable);
		void FromQuery(string value);
		string ToQuery();
	}
}