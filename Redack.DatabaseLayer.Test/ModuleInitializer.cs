using Redack.Test.Lollipop.Configuration;

namespace Redack.Test.Lollipop
{
    internal static class ModuleInitializer
    {
        internal static void Run()
        {
            EffortProviderFactory.Register();
        }
    }
}