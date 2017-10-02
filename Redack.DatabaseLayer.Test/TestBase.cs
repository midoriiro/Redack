using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redack.DatabaseLayer.Test.DataAccess;

namespace Redack.DatabaseLayer.Test
{
    public class TestBase
    {
        public TestBase()
        {
            EffortProviderFactory.ResetDb();
        }
    }
}
