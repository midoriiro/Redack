using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/apikeys")]
    public class ApiKeysController : RepositoryApiController<ApiKey> {}
}