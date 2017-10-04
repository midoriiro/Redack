﻿using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Redack.Test.Lollipop.Specimen
{
    public class IgnorePropertiesSpecimen : ISpecimenBuilder
    {
        private readonly string[] _propertiesName;

        public IgnorePropertiesSpecimen(string[] propertiesName)
        {
            this._propertiesName = propertiesName;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            PropertyInfo propertyInfo = request as PropertyInfo;

            if (propertyInfo != null && this._propertiesName.Contains(propertyInfo.Name))
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}
