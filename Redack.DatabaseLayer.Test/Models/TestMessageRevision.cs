using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DomainLayer.Models;
using Redack.Test.Lollipop;
using Xunit;

namespace Redack.DatabaseLayer.Test.Models
{
    public class TestMessageRevision : BaseTest
    {
        [Fact]
        public void Valid()
        {
            var message = this.CreateMessage();
            var revision = this.CreateMessageRevision(message: message);

            using (var repository = this.CreateRepository<MessageRevision>())
            {
                Assert.IsNotNull(repository.GetById(revision.Id));
            }
        }

        [Fact]
        public void WithoutEditor()
        {
            var message = this.CreateMessage();

            var revision = this.CreateMessageRevision(message: message, push: false);
            revision.Editor = null;

            using (var repository = this.CreateRepository<MessageRevision>())
            {
                repository.Insert(revision);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithoutMessage()
        {
            var revision = this.CreateMessageRevision(push: false);
            revision.Message = null;

            using (var repository = this.CreateRepository<MessageRevision>())
            {
                repository.Insert(revision);

                Assert.ThrowsException<DbEntityValidationException>(() => repository.Commit());
            }
        }

        [Fact]
        public void WithEditorCascadeDelete()
        {
            /*var user1 = this.CreateUser();
            var user2 = this.CreateUser();

            var message1 = this.CreateMessage(user1);
            var message2 = this.CreateMessage(user2);

            var revision1 = this.CreateMessageRevision(user1, message1);
            var revision2 = this.CreateMessageRevision(user2, message2);

            using (var repository = this.CreateRepository<MessageRevision>())
            {
                repository.Delete(revision1);
                repository.Commit();

                Assert.IsNull(repository.GetById(revision1.Id));
                Assert.IsNotNull(repository.GetById(revision2.Id));
            }

            using (var repository = this.CreateRepository<User>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Messages.Any(
                        p => p.Revisions.Any(
                            r => r.Id == revision1.Id))));

                Assert.IsNotNull(repository.GetById(user1.Id));
                Assert.IsNotNull(repository.GetById(user2.Id));
            }

            Assert.AreEqual(0, message1.Revisions.Count);
            Assert.AreEqual(1, message2.Revisions.Count);*/

            Assert.IsTrue(false);
        }

        [Fact]
        public void WithMessageCascadeDelete()
        {
            /*var user1 = this.CreateUser();
            var user2 = this.CreateUser();

            var message1 = this.CreateMessage(user1);
            var message2 = this.CreateMessage(user2);

            var revision1 = this.CreateMessageRevision(user1, message1);
            var revision2 = this.CreateMessageRevision(user2, message2);

            using (var repository = this.CreateRepository<MessageRevision>())
            {
                repository.Delete(revision1);
                repository.Commit();

                Assert.IsNull(repository.GetById(revision1.Id));
                Assert.IsNotNull(repository.GetById(revision2.Id));
            }

            using (var repository = this.CreateRepository<Message>())
            {
                Assert.IsFalse(repository.All().Any(
                    e => e.Revisions.Any(
                        p => p.Id == revision1.Id)));

                Assert.IsNotNull(repository.GetById(message1.Id));
                Assert.IsNotNull(repository.GetById(message2.Id));
            }

            Assert.AreEqual(0, message1.Revisions.Count);
            Assert.AreEqual(1, message2.Revisions.Count);*/

            Assert.IsTrue(false);
        }
    }
}