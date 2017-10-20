using Redack.DomainLayer.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Redack.DatabaseLayer.DataAccess
{
    public interface IDbContext
    {
        DbContextConfiguration Configuration { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : Entity;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : Entity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        int Rollback();
        void SetEntityState(Entity entity, EntityState state);
        void SetEntryState(DbEntityEntry entry, EntityState state);
        void Dispose();
    }
}