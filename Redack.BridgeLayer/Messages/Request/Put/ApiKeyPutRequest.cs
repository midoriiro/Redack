using System.ComponentModel.DataAnnotations;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Request.Put
{
	public abstract class ApiKeyPutRequest : BasePutRequest<ApiKey>
	{
		[Required]
		public string Key { get; set; }
	}
}