﻿using Redack.DomainLayer.Models;
using System.Web.Http;

namespace Redack.ServiceLayer.Controllers
{
    [RoutePrefix("api/threads")]
    public class ThreadsController : RepositoryApiController<Thread> {}
}