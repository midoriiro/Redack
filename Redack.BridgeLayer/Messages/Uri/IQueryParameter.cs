using System.Linq;
using System.Net.Http;

namespace Redack.BridgeLayer.Messages.Uri
{
	public interface IQueryParameter
	{
		bool IsUnique { get; }

		QueryBuilder Builder { get; set; }
		HttpRequestMessage Request { get; set; }

		IQueryable Execute(string key, IQueryable queryable);
		void FromQuery(string value);
		string ToQuery();
	}
}