using System.Data.Entity;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.DataAccess
{
    public class RedackDbContext<T> : DbContext where T : Entity
    {
        public DbSet<T> Entities { get; set; }

        public RedackDbContext() : base("RedackDbContext") {}
    }
}
