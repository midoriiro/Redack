using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestThread : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var thread = this.CreateThread();

            using (var repository = this.CreateRepository<Thread>())
            {
                Assert.IsNotNull(repository.GetById(thread.Id));
            }
        }

        [Fact]
        public void WithoutTitle()
        {
            var thread = this.CreateThread(push: false);
            thread.Title = null;

            using (var repository = this.CreateRepository<Thread>())
            {
                repository.Insert(thread);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutNode()
        {
            var thread = this.CreateThread(push: false);
            thread.Node = null;

            using (var repository = this.CreateRepository<Thread>())
            {
                repository.Insert(thread);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithNodeCascadeDelete()
        {
            var node1 = this.CreateNode();
            var node2 = this.CreateNode();

            var thread1 = this.CreateThread(node1);
            var thread2 = this.CreateThread(node1);
            var thread3 = this.CreateThread(node2);

            using (var repository = this.CreateRepository<Thread>())
            {
                repository.Delete(thread1);
                repository.Commit();

                Assert.IsNull(repository.GetById(thread1.Id));
                Assert.IsNotNull(repository.GetById(thread2.Id));
                Assert.IsNotNull(repository.GetById(thread3.Id));
            }

            using (var repository = this.CreateRepository<Node>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Threads.Any(
                        p => p.Id == thread1.Id)));

                Assert.IsNotNull(repository.GetById(node1.Id));
                Assert.IsNotNull(repository.GetById(node2.Id));
            }

            Assert.AreEqual(1, node1.Threads.Count);
            Assert.AreEqual(1, node2.Threads.Count);
        }

        [Fact]
        public void WithMessagesCascadeDelete()
        {
            var node1 = this.CreateNode();
            var node2 = this.CreateNode();

            var thread1 = this.CreateThread(node1);
            var thread2 = this.CreateThread(node2);

            var message1 = this.CreateMessage(thread: thread1);
            var message2 = this.CreateMessage(thread: thread1);
            var message3 = this.CreateMessage(thread: thread2);

            using (var repository = this.CreateRepository<Thread>())
            {
                repository.Delete(thread1);
                repository.Commit();

                Assert.IsNull(repository.GetById(thread1.Id));
                Assert.IsNotNull(repository.GetById(thread2.Id));
            }

            using (var repository = this.CreateRepository<Message>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Thread.Id == thread1.Id));

                Assert.IsNull(repository.GetById(message1.Id));
                Assert.IsNull(repository.GetById(message2.Id));
                Assert.IsNull(repository.GetById(message3.Id));
            }

            Assert.AreEqual(0, thread1.Messages.Count);
            Assert.AreEqual(1, thread2.Messages.Count);
        }
    }
}