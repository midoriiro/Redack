using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.DatabaseLayer.Test.Data
{
    public class DummyDbContext : RedackDbContext
    {
        public virtual DbSet<DummyEntity> Dummies { get; set; }

        public DummyDbContext() : base("fakeConnectionString")
        {
        }
    }
}
