using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Models.Request
{
	public abstract class ApiKeyPutRequest : BasePutRequest<ApiKey>
	{
		[Required]
		public string Key { get; set; }
	}
}