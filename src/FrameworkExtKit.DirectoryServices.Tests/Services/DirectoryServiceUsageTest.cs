using FrameworkExtKit.Services.DirectoryServices;
using NUnit.Framework;
using System.Linq;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {
    [TestFixture]
	[Author("Yufei Liu", "yliu@leyun.co.uk")]
    public class DirectoryServiceUsageTest {

        protected IDirectoryAccountService service;

        [SetUp]
        public void SetUp() {
            service = new LdapDirectoryAccountService();
        }

        [Test]
        [Ignore("will be moved to distribution list service test")]
        public void test_search_distribution_list() {
            var account = service.FindByAlias("ITReporter");
            Assert.AreEqual("ITReporter", account.Alias);
            Assert.IsTrue(account.Subscriptions.Length > 0);

            string[] members = new string[]{
                "dkulkarni3", "LYufei"
            };

            for (int i = 0; i < members.Length; i++) {
                Assert.AreEqual("alias=" + members[i].ToLower(), account.Subscriptions[i].ToLower());
            }
        }

        //[Test]
        //[Ignore("will be moved to distribution list service test")]
        //public void test_search_distribution_list2() {
        //    var distribution_lists = service.SearchDistributionList("ITReporter");
        //    Assert.AreEqual(1, distribution_lists.Count());

        //    var entry = distribution_lists.FirstOrDefault();

        //    Assert.AreEqual(8, entry.Subscriptions.Length);
        //    string[] members = new string[]{
        //        "dkulkarni3", "LYufei"
        //    };

        //    for (int i = 0; i < members.Length; i++) {
        //        Assert.AreEqual("alias=" + members[i].ToLower(), entry.Subscriptions[i].ToLower());
        //    }
        //}
    }
}
