using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.AutoEF;
using Ploeh.AutoFixture;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.DatabaseLayer.Test.Sugar.Customization
{
    class EntityCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(
                new AutoFixture.AutoEF.EntityCustomization(
                    new DbContextEntityTypesProvider(typeof(RedackDbContext))));
        }
    }
}
