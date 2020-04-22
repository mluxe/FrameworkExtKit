using FrameworkExtKit.Services.DirectoryServices;
using FrameworkExtKit.UnitTest.Services.DirectoryService;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {

    [TestFixture]
	[Author("Yufei Liu", "yliu@leyun.co.uk")]
    public class MemoryDirectoryAccountServiceTest : DirectoryAccountServiceTest {


        public MemoryDirectoryAccountServiceTest() {
            service = new MemoryDirectoryAccountService();
        }

        [Test]
        public override void test_find_direct_manager() {
            DirectoryAccount user = service.FindByAlias(userAlias);

            DirectoryAccount manager = service.FindDirectManager(user);

            Assert.IsNotNull(manager);
            Assert.AreEqual(user.DirectManager, manager.DistinguishName);
        }

        [Test]
        public void test_find_by_alias() {
            DirectoryAccount user = service.FindByAlias(userAlias);

            IEnumerable<DirectoryAccount> accounts = service.FindFunctionManagers(user);

            Assert.IsNotNull(accounts);
            Assert.AreEqual(0, accounts.Count());
        }

        [Test]
        public void test_find_team_members2() {
            DirectoryAccount user = service.FindByAlias("KLi5");

            IEnumerable<DirectoryAccount> accounts = service.FindTeamMembers(user);

            Assert.IsNotNull(accounts);
            Assert.IsTrue(accounts.Count() > 0);
            Assert.AreEqual(2, accounts.Count());
        }

        protected override void test_ldap_record_yufei(DirectoryAccount user) {
            var msg = this.GetType().ToString() + " unexpected value";
            Assert.IsNotNull(user, this.GetType().ToString() + " - user cannot be null");
            Assert.AreEqual("contractor", user.EmployeeType, msg);
            Assert.AreEqual("LYufei", user.Alias, msg);

            Assert.AreEqual("liu-20140317", user.UniqueId, msg);
            Assert.AreEqual("Yufei", user.GivenName, msg);
            Assert.AreEqual("Liu", user.SurName, msg);
            Assert.AreEqual("Liu Yufei  721668", user.CommonName, msg);
            Assert.AreEqual(1, user.Managers.Length, msg);
            Assert.AreEqual("IT", user.Organization, msg);
            Assert.AreEqual("IT Operations", user.OrganizationUnit, msg);
            Assert.AreEqual("LYufei@sample.com", user.Mail, msg);
            Assert.AreEqual("cn=liu yufei  721668,ou=contractor,o=sample,c=an", user.DistinguishName.ToLower(), msg);
            Assert.AreEqual("Yufei Liu", user.DisplayName, msg);
            Assert.AreEqual("contractor", user.EmployeeType, msg);
            Assert.AreEqual("56787840", user.GIN, msg);
            Assert.AreEqual("+44 1293 557048", user.TelephoneNumbers[0], msg);
            Assert.AreEqual("Engineer", user.JobTitle, msg);
            Assert.AreEqual("IT", user.Department, msg);
            Assert.AreEqual("liu-20140317", user.UniqueId, msg);
            Assert.AreEqual(721668, user.Id, msg);
            Assert.AreEqual(1, user.Managers.Length, msg);
            Assert.AreEqual("RH6 6NZ", user.PostalCode, msg);
            Assert.AreEqual("Buckingham Gate,  West Sussex", user.Street, msg);
            Assert.AreEqual("GB", user.Country, msg);
            Assert.AreEqual("Gatwick", user.City, msg);
            Assert.IsTrue(user.Subscriptions.Length > 0, msg);
            Assert.AreEqual("GB0080", user.ITBuilding, msg);
            Assert.AreEqual("", user.Geosite, msg);
            //Assert.AreEqual("", user.DirectManagerDirectoryID);


            //information that's not available on AD
            Assert.AreEqual("GB0008", user.LocationCode, msg);
            Assert.AreEqual("82154011-JC", user.JobCode, msg);
            Assert.AreEqual("82154011", user.JobCodeID, msg);
            Assert.AreEqual("JC", user.JobCodeName, msg);
            Assert.AreEqual("IT Operations", user.BusinessCategory, msg);
            Assert.AreEqual(1, user.AccessRights.Length, msg);
            Assert.AreEqual("Data Processing", user.JobCategory, msg);
            Assert.AreEqual("00001530-JG", user.JobGroup, msg);
            Assert.AreEqual("00001530", user.JobGroupId, msg);
            Assert.AreEqual("JG", user.JobGroupName, msg);
            Assert.AreEqual("", user.AccountingCode, msg);
            Assert.AreEqual("0004482751", user.AccountingUnit, msg);
        }

    }
}
