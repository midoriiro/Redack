using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public class RedackDbContext : DbContext, IDbContext
    {
        public virtual DbSet<ApiKey> ApiKeys { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Identity> Identities { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageHistory> MessageHistories { get; set; }
        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public RedackDbContext() : base(
            ConfigurationManager.ConnectionStrings["RedackDbConnection"].ConnectionString)
        {
        }

        public RedackDbContext(DbConnection connection) : base(connection, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasRequired(e => e.Credential)
                .WithRequiredPrincipal()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithOptional(e => e.Author)
                .Map(e => e.MapKey("Author_Id"))
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Credential>()
                .HasOptional(e => e.ApiKey)
                .WithOptionalPrincipal()
                .Map(e => e.MapKey("Credential_Id"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Client>()
                .HasOptional(e => e.ApiKey)
                .WithOptionalPrincipal()
                .Map(e => e.MapKey("Client_Id"))
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Permissions)
                .WithMany(e => e.Users)
                .Map(e => e
                    .MapLeftKey("User_Id")
                    .MapRightKey("Permission_Id")
                    .ToTable("UserPermissions"));

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Permissions)
                .WithMany(e => e.Groups)
                .Map(e => e
                    .MapLeftKey("Group_Id")
                    .MapRightKey("Permission_Id")
                    .ToTable("GroupPermissions"));

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Groups)
                .Map(e => e
                    .MapLeftKey("Group_Id")
                    .MapRightKey("User_Id")
                    .ToTable("GroupUsers"));
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
