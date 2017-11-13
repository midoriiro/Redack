using System;
using Redack.DomainLayer.Models;

namespace Redack.ServiceLayer.Controllers
{
	public interface IOwnerFilter
	{
		bool IsOwner(int id);
	}
}