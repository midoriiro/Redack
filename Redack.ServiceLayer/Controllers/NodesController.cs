﻿using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/nodes")]
    public class NodesController : RepositoryApiController<Node> {}
}