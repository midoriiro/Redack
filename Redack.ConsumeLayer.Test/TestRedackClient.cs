using Redack.Test.Lollipop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Redack.ConsumeLayer.Test
{
	public class TestRedackClient : BaseTest
	{
		public TestRedackClient()
		{
			this.InitialyzeServer();
		}

		[Fact]
		public void Valid()
		{
			var client = new RedackClient("lol", "localhost", false, this.Server);
		}
	}
}
