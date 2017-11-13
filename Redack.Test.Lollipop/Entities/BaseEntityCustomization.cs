using System.Security.Cryptography;
using System.Threading;
using Ploeh.AutoFixture;
using Redack.Test.Lollipop.Customizations;

namespace Redack.Test.Lollipop.Entities
{
	public class BaseEntityCustomization : ICustomization
	{
		public virtual void Customize(IFixture fixture)
		{
			// TODO: fix this workaround
			
			//Thread.Sleep(50);
		}
	}
}
