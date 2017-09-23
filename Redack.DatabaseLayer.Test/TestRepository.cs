using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Model;

namespace Redack.DatabaseLayer.Test
{
    [TestClass]
    public class TestRepository
    {
        private User User { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.User = new User()
            {
                Id = 0,
                Alias = "aaa",
                Credential = new Credential()
                {
                    Login = "aaa@aaa.com",
                    Password = "aaaaaaaa",
                    PasswordConfirm = "aaaaaaaa"
                }
            };

            using (var repo = new Repository<User>())
            {
                repo.Insert(this.User);
                repo.Commit();
            }
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            using (var repo = new Repository<User>())
            {
                repo.Delete(this.User);
            }
        }

        [TestMethod]
        public void TestGetEntity()
        {
            using (var repo = new Repository<User>())
            {
                User actualUser = repo.Get(this.User.Id);
                Assert.AreEqual(this.User.Id, actualUser.Id);
            }
        }
    }
}
