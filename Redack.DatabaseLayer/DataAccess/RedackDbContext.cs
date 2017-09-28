using System.Data.Entity;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public class RedackDbContext : DbContext
    {
        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public RedackDbContext(string connectionString) : base(connectionString) {}
    }
}
