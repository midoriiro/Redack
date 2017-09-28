using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Redack.DatabaseLayer.Test.Sugar.Attribute;
using Redack.DatabaseLayer.Test.Sugar.Customization;

namespace Redack.DatabaseLayer.Test.Sugar.Attribute
{
    class IgnorePropertiesAttribute : AutoDataAttribute
    {
        public IgnorePropertiesAttribute(string[] propertiesName) :
            base(new Fixture().Customize(new IgnorePropertiesCustomization(propertiesName)))
        {
        }
    }
}
