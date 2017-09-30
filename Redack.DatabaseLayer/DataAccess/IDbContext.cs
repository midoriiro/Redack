﻿using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : Entity;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : Entity;
        int SaveChanges();
        int Rollback();
        void SetEntityState(Entity entity, EntityState state);
        void SetEntryState(DbEntityEntry entry, EntityState state);
        void Dispose();
    }
}