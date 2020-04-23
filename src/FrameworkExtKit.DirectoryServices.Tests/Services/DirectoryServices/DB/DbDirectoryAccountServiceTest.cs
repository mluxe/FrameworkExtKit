using FrameworkExtKit.Services.DirectoryServices;
using FrameworkExtKit.Services.DirectoryServices.DB;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkExtKit.Services.Tests.DirectoryServices.DB {
    [TestFixture]
	[Author("Yufei Liu", "feilfly@gmail.com")]
    [Ignore("to run this unit test, please prepare the database and import data from LDAPUsers.sql, or create your own unit tests")]
    public class DbDirectoryAccountServiceTest : DirectoryAccountServiceTest {

        [SetUp]
        public override void SetUp() {
            service = new DbDirectoryAccountService("DBDirectoryService");
            base.SetUp();
        }

        [Test]
        public override void test_find_team_members() {
            DirectoryAccount user = service.FindByAlias("KLi5");

            IEnumerable<DirectoryAccount> accounts = service.FindTeamMembers(user);

            Assert.IsNotNull(accounts);
            Assert.IsTrue(accounts.Count() > 0);
            // we use >= 0 assertion is because 
            // the There is only 1 team members in the testing db data
            Assert.IsTrue(accounts.Count() >= 0);
        }

        protected override void test_ldap_record_yufei(DirectoryAccount user) {
            Assert.IsNotNull(user);

            Assert.Multiple(() => {
                Assert.AreEqual("contractor", user.EmployeeType);
                Assert.AreEqual("LYufei", user.Alias);

                Assert.AreEqual("liu-20140317", user.UniqueId);
                Assert.AreEqual("Yufei", user.GivenName);
                Assert.AreEqual("Liu", user.SurName);
                Assert.AreEqual("Yufei Liu", user.CommonName);
                Assert.AreEqual(1, user.Managers.Length);
                Assert.AreEqual("IT", user.Organization);
                Assert.AreEqual("Shared Services", user.OrganizationUnit);
                Assert.AreEqual("LYufei@company.com", user.Mail);
                Assert.AreEqual("cn=Liu Yufei  100001,ou=contractor,o=company,C=AN", user.DistinguishName);
                Assert.AreEqual("Yufei Liu", user.DisplayName);
                Assert.AreEqual("contractor", user.EmployeeType);
                Assert.AreEqual("56787840", user.GIN);
                Assert.AreEqual("+44 1293 557048", user.TelephoneNumbers[0]);
                Assert.AreEqual("Contractor", user.JobTitle);
                Assert.AreEqual("IT", user.Department);
                Assert.AreEqual("liu-20140317", user.UniqueId);
                Assert.AreEqual(721668, user.Id);
                Assert.AreEqual(1, user.Managers.Length);
                Assert.AreEqual("RH6 6HR", user.PostalCode);
                Assert.AreEqual("West Sussex", user.Street);
                Assert.AreEqual("GB", user.Country);
                Assert.AreEqual("Gatwick", user.City);
                Assert.AreEqual(1, user.Subscriptions.Length);
                Assert.AreEqual("Gatwick", user.ITBuilding);
                Assert.AreEqual("GB0001", user.Geosite);
                Assert.AreEqual("IT Ops", user.SegmentCode);
                //Assert.AreEqual("", user.DirectManagerDirectoryID);


                //information that's not available on AD
                Assert.AreEqual("GB0008", user.LocationCode);
                Assert.AreEqual("82154011-JC", user.JobCode);
                Assert.AreEqual("82154011", user.JobCodeID);
                Assert.AreEqual("JC", user.JobCodeName);
                Assert.AreEqual("IT Ops", user.BusinessCategory);
                Assert.AreEqual(3, user.AccessRights.Length);
                Assert.AreEqual("CN=Liu Yufei  100001,ou=Users,OU=Horsham,OU=GB,DC=DIR,DC=company,DC=com".ToLower(), user.ActiveDirectoryDN.ToLower());
                Assert.AreEqual("Data Processing", user.JobCategory);
                Assert.AreEqual("00001540-JG", user.JobGroup);
                Assert.AreEqual("00001540", user.JobGroupId);
                Assert.AreEqual("JG", user.JobGroupName);
                Assert.AreEqual("IT Operations", user.LegalEntity);
            });
            
        }
    }
}
