using FrameworkExtKit.Services.DirectoryServices;
using FrameworkExtKit.Services.DirectoryServices.DB;
using NUnit.Framework;
using System;
using System.Configuration;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {
    [TestFixture]
	[Author("Yufei Liu", "yliu@leyun.co.uk")]
    public class DirectoryAccountServiceInstanceCreationTest {
        [Test]
        [Ignore("code migration not completed")]
        public void create_ldap_directory_service_test() {
            /*
            ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"] = "FrameworkExtKit.Services.DirectoryServices.LdapDirectoryService";
            ConfigurationManager.AppSettings["DirectoryAccountService.RootEntry"] = "LDAP://ldap.company.com/o=company,c=an";
            DirectoryAccountService.CurrentService = null;
            IDirectoryService service = DirectoryAccountService.GetUserDirectoryInstance();
            Assert.IsNotNull(service);
            Assert.IsTrue(service is LdapDirectoryService);
            */
        }

        [Test]
        [Ignore("code migration not completed")]
        public void create_active_directory_service_test() {
            /*
            ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"] = "FrameworkExtKit.Services.DirectoryServices.ActiveDirectoryService";
            ConfigurationManager.AppSettings["DirectoryAccountService.RootEntry"] = "LDAP://DIR.company.com";
            DirectoryAccountService.CurrentService = null;
            IDirectoryService service = DirectoryAccountService.GetUserDirectoryInstance();
            Assert.IsNotNull(service);
            Assert.IsTrue(service is ActiveDirectoryService);
            */
        }

        [Test]
        [Ignore("code migration not completed")]
        public void create_db_directory_service_test() {
            /*
            ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"] = "FrameworkExtKit.Services.DirectoryServices.DB.DbDirectoryService";
            ConfigurationManager.AppSettings["DirectoryAccountService.RootEntry"] = "DB://DBDirectoryService";
            DirectoryAccountService.CurrentService = null;
            IDirectoryService service = DirectoryAccountService.GetUserDirectoryInstance();
            Assert.IsNotNull(service);
            Assert.IsTrue(service is DbDirectoryService);
            */
        }

        [Test]
        [Ignore("code migration not completed")]
        public void create_invalid_directory_service_test() {
            /*
            ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"] = "";
            ConfigurationManager.AppSettings["DirectoryAccountService.RootEntry"] = "DB://DBDirectoryService";
            DirectoryAccountService.CurrentService = null;
            try {
                IDirectoryService service = DirectoryAccountService.GetUserDirectoryInstance();
                Assert.Fail();
            } catch (Exception e) {
                Assert.AreEqual("DirectoryAccountService.ClassName is not found in the application or web config. \n Please add <add key=\"DirectoryAccountService.ClassName\" value=\"xyz\" /> to the config file.", e.Message);
                Assert.IsTrue(true);
            }
            */
        }
    }
}
