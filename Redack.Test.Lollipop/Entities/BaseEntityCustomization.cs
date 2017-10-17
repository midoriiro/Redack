using System.Threading;
using Ploeh.AutoFixture;

namespace Redack.Test.Lollipop.Entities
{
    public class BaseEntityCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            // TODO: fix this workaround

            Thread.Sleep(25);
        }
    }
}
