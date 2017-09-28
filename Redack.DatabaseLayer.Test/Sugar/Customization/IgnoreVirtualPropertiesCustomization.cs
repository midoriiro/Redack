using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.Test.Sugar.Specimen;

namespace Redack.DatabaseLayer.Test.Sugar.Customization
{
    class IgnoreVirtualPropertiesCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualProperties());
        }
    }
}
