using System.Data.Common;
using System.Web.Http;

namespace Redack.ServiceLayer
{
	public class HttpApiConfiguration : HttpConfiguration
	{
		public DbConnection DbConnection { get; set; } = null;
	}
}