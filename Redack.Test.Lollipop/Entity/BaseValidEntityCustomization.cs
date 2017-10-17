using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ploeh.AutoFixture;

namespace Redack.Test.Lollipop.Entity
{
    public class BaseValidEntityCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            // TODO: fix this workaround

            Thread.Sleep(25);
        }
    }
}
