using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
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

        public RedackDbContext(string connectionString) : base(connectionString) {}
        

        public new DbSet<TEntity> Set<TEntity>() where TEntity : Entity
        {
            return this.Set<TEntity>();
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : Entity
        {
            return this.Entry<TEntity>(entity);
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
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
