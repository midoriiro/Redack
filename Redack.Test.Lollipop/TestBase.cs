using Redack.Test.Lollipop.Configuration;

namespace Redack.Test.Lollipop
{
    public class TestBase
    {
        public TestBase()
        {
            EffortProviderFactory.ResetDb();
        }
    }
}
