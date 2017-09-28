using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.Test.Sugar.Specimen;

namespace Redack.DatabaseLayer.Test.Sugar.Customization
{
    class IgnorePropertiesCustomization : ICustomization
    {
        private readonly string[] _propertiesName;

        public IgnorePropertiesCustomization(string[] propertiesName)
        {
            _propertiesName = propertiesName;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreProperties(this._propertiesName));
        }
    }
}
