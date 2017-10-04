using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public class RedackDbContext : DbContext, IDbContext
    {
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public RedackDbContext(string connectionString) : base(connectionString)
        {
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : Entity
        {
            return base.Set<TEntity>();
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : Entity
        {
            return base.Entry<TEntity>(entity);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                var fullErrorMessage = string.Join("; ", errorMessages);

                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public int Rollback()
        {
            int count = 0;

            foreach (DbEntityEntry entry in this.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        count++;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        count++;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        count++;
                        break;
                }
            }

            return count;
        }

        public void SetEntityState(Entity entity, EntityState state)
        {
            this.Entry(entity).State = state;
        }

        public void SetEntryState(DbEntityEntry entry, EntityState state)
        {
            entry.State = state;
        }
    }
}
