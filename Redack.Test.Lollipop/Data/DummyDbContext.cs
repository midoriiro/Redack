﻿using System.Data.Entity;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.Test.Lollipop.Data
{
    public class DummyDbContext : RedackDbContext
    {
        public virtual DbSet<DummyEntity> Dummies { get; set; }

        public DummyDbContext() : base("fakeConnectionString")
        {
        }
    }
}
