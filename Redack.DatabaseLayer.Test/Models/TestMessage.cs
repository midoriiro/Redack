using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestMessage : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var message = this.CreateMessage();

            using (var repository = this.CreateRepository<Message>())
            {
                Assert.IsNotNull(repository.GetById(message.Id));
            }
        }

        [Fact]
        public void WithoutText()
        {
            var message = this.CreateMessage(push: false);
            message.Text = null;

            using (var repository = this.CreateRepository<Message>())
            {
                repository.Insert(message);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutThread()
        {
            var message = this.CreateMessage(push: false);
            message.Thread = null;

            using (var repository = this.CreateRepository<Message>())
            {
                repository.Insert(message);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutAuthor()
        {
            var message = this.CreateMessage(push: false);
            message.Author = null;

            using (var repository = this.CreateRepository<Message>())
            {
                repository.Insert(message);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithUserCascadeDelete()
        {
            var user1 = this.CreateUser();
            var user2 = this.CreateUser();

            var message1 = this.CreateMessage(user1);
            var message2 = this.CreateMessage(user1);
            var message3 = this.CreateMessage(user2);

            using (var repository = this.CreateRepository<Message>())
            {
                repository.Delete(message1);
                repository.Commit();

                Assert.IsNull(repository.GetById(message1.Id));
                Assert.IsNotNull(repository.GetById(message2.Id));
                Assert.IsNotNull(repository.GetById(message3.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Messages.Any(
                        p => p.Id == message1.Id)));

                Assert.IsNotNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
            }

            Assert.AreEqual(1, user1.Messages.Count);
            Assert.AreEqual(1, user2.Messages.Count);
        }

        [Fact]
        public void WithThreadCascadeDelete()
        {
            var thread1 = this.CreateThread();
            var thread2 = this.CreateThread();

            var message1 = this.CreateMessage(thread: thread1);
            var message2 = this.CreateMessage(thread: thread1);
            var message3 = this.CreateMessage(thread: thread2);

            using (var repository = this.CreateRepository<Message>())
            {
                repository.Delete(message1);
                repository.Commit();

                Assert.IsNull(repository.GetById(message1.Id));
                Assert.IsNotNull(repository.GetById(message2.Id));
                Assert.IsNotNull(repository.GetById(message3.Id));
            }

            using (var repository = this.CreateRepository<Thread>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Messages.Any(
                        p => p.Id == message1.Id)));

                Assert.IsNotNull(repository.GetById(thread1.Id));
                Assert.IsNotNull(repository.GetById(thread2.Id));
            }

            Assert.AreEqual(1, thread1.Messages.Count);
            Assert.AreEqual(1, thread2.Messages.Count);
        }

        [Fact]
        public void WithRevisionsCascadeDelete()
        {
            var message1 = this.CreateMessage();
            var message2 = this.CreateMessage();

            var revision1 = this.CreateMessageRevision(message: message1);
            var revision2 = this.CreateMessageRevision(message: message1);
            var revision3 = this.CreateMessageRevision(message: message2);

            using (var repository = this.CreateRepository<Message>())
            {
                repository.Delete(message1);
                repository.Commit();

                Assert.IsNull(repository.GetById(message1.Id));
                Assert.IsNotNull(repository.GetById(message2.Id));
            }

            using (var repository = this.CreateRepository<MessageRevision>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Message.Id == message1.Id));

                Assert.IsNull(repository.GetById(revision1.Id));
                Assert.IsNull(repository.GetById(revision2.Id));
                Assert.IsNotNull(repository.GetById(revision3.Id));
            }

            Assert.AreEqual(0, message1.Revisions.Count);
            Assert.AreEqual(1, message2.Revisions.Count);
        }
    }
}