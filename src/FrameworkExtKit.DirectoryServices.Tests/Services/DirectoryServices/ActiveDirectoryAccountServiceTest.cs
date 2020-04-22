using FrameworkExtKit.Services.DirectoryServices;
using NUnit.Framework;
using System;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {

    [TestFixture]
	[Author("Yufei Liu", "yliu@leyun.co.uk")]
    [Ignore("please update the unit test to fit your ldap structure")]
    public partial class ActiveDirectoryAccountServiceTest : DirectoryAccountServiceTest {

        [Test]
        public void test_active_directory_find_direct_manager() {
            DirectoryAccount user = service.FindByAlias(userAlias);

            DirectoryAccount manager = service.FindDirectManager(user);

            Assert.IsNotNull(manager);
            Assert.AreEqual(user.DirectManager, manager.DistinguishName);
        }

        [Test]
        public override void test_find_by_identity_should_success() {
            // TODO: complete the test
            Assert.IsTrue(true);
        }

        protected override void test_ldap_record_yufei(DirectoryAccount user) {
            var msg = this.GetType().ToString() + " unexpected value";
            Assert.IsNotNull(user, this.GetType().ToString() + " - user cannot be null");
            Assert.AreEqual("contractor", user.EmployeeType, msg);
            Assert.AreEqual("LYufei", user.Alias, msg);

            Assert.AreEqual("liu-20140317", user.UniqueId, msg);
            Assert.AreEqual("Yufei", user.GivenName, msg);
            Assert.AreEqual("Liu", user.SurName, msg);
            Assert.AreEqual("Liu Yufei  100001", user.CommonName, msg);
            Assert.AreEqual(1, user.Managers.Length, msg);
            Assert.AreEqual("Oilfield", user.Organization, msg);
            Assert.AreEqual("Legacy ITE - INF Report & Config Systems", user.OrganizationUnit, msg);
            Assert.AreEqual("LYufei@company.com", user.Mail, msg);
            Assert.AreEqual("CN=Liu Yufei  100001,OU=Users,OU=Gatwick-GB0080,OU=GB,OU=EAF,DC=DIR,DC=company,DC=com", user.DistinguishName, msg);
            Assert.AreEqual("Yufei Liu", user.DisplayName, msg);
            Assert.AreEqual("contractor", user.EmployeeType, msg);
            Assert.AreEqual("04878765", user.GIN, msg);
            Assert.AreEqual("441293557048", user.TelephoneNumbers[0], msg);
            Assert.AreEqual("Contractor", user.JobTitle, msg);
            Assert.AreEqual("IT", user.Department, msg);
            Assert.AreEqual("liu-20140317", user.UniqueId, msg);
            Assert.AreEqual(721668, user.Id, msg);
            Assert.AreEqual(1, user.Managers.Length, msg);
            Assert.AreEqual("RH6 0NZ", user.PostalCode, msg);
            Assert.AreEqual("Buckingham Gate,  West Sussex", user.Street, msg);
            Assert.AreEqual("GB", user.Country, msg);
            Assert.AreEqual("Gatwick", user.City, msg);
            Assert.AreEqual(0, user.Subscriptions.Length, msg);

            //information that's not available on AD
            Assert.AreEqual(String.Empty, user.LocationCode, msg);
            Assert.AreEqual(String.Empty, user.JobCode, msg);
            Assert.AreEqual("O611-IT Infrastructure", user.BusinessCategory, msg);
            Assert.AreEqual(0, user.AccessRights.Length, msg);
            Assert.AreEqual(String.Empty, user.ActiveDirectoryDN.ToLower(), msg);
            //Assert.AreEqual(0, user.EDMWorkStations.Length, msg);
            Assert.AreEqual(String.Empty, user.JobCategory, msg);
            Assert.AreEqual(String.Empty, user.JobGroup, msg);
            Assert.AreEqual(0, user.Subscriptions.Length, msg);
            Assert.AreEqual(String.Empty, user.ITBuilding, msg);
            Assert.AreEqual(String.Empty, user.Geosite, msg);
            Assert.AreEqual(String.Empty, user.AccountingCode, msg);
            Assert.AreEqual(String.Empty, user.AccountingUnit, msg);
            Assert.AreEqual(String.Empty, user.LegalEntity);
        }
    }
}
