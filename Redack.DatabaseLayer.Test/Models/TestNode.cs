using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestNode : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var node = this.CreateNode();

            using (var repository = this.CreateRepository<Node>())
            {
                Assert.IsNotNull(repository.GetById(node.Id));
            }
        }

        [Fact]
        public void WithoutName()
        {
            var node = this.CreateNode(false);
            node.Name = null;

            using (var repository = this.CreateRepository<Node>())
            {
                repository.Insert(node);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithThreadsCascadeDelete()
        {
            var node1 = this.CreateNode();
            var node2 = this.CreateNode();

            var thread1 = this.CreateThread(node1);
            var thread2 = this.CreateThread(node1);
            var thread3 = this.CreateThread(node2);

            using (var repository = this.CreateRepository<Node>())
            {
                repository.Delete(node1);
                repository.Commit();

                Assert.IsNull(repository.GetById(node1.Id));
                Assert.IsNotNull(repository.GetById(node2.Id));
            }

            using (var repository = this.CreateRepository<Thread>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Node.Id == node1.Id));

                Assert.IsNull(repository.GetById(thread1.Id));
                Assert.IsNull(repository.GetById(thread2.Id));
                Assert.IsNotNull(repository.GetById(thread3.Id));
            }

            Assert.AreEqual(0, node1.Threads.Count);
            Assert.AreEqual(1, node2.Threads.Count);
        }
    }
}