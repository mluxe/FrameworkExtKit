using FrameworkExtKit.Services.DirectoryServices;
using FrameworkExtKit.Services.DirectoryServices.DB;
using NUnit.Framework;
using System;
using System.Configuration;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {
    [TestFixture]
	[Author("Yufei Liu", "yliu@leyun.co.uk")]
    public class DefaultDirectoryServiceFactoryTest {
        [Test]
        public void create_ldap_directory_service_test() {
            ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"] = "FrameworkExtKit.Services.DirectoryServices.LdapDirectoryAccountService";
            ConfigurationManager.AppSettings["DirectoryAccountService.RootEntry"] = "LDAP://ldap.company.com/o=company,c=an";
            IDirectoryAccountService service = DefaultDirectoryServiceFactory.GetDirectoryAccountService();
            Assert.IsNotNull(service);
            Assert.IsTrue(service is LdapDirectoryAccountService);
        }

        [Test]
        public void create_active_directory_service_test() {
            ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"] = "FrameworkExtKit.Services.DirectoryServices.ActiveDirectoryAccountService";
            ConfigurationManager.AppSettings["DirectoryAccountService.RootEntry"] = "LDAP://DIR.company.com";
            IDirectoryAccountService service = DefaultDirectoryServiceFactory.GetDirectoryAccountService();
            Assert.IsNotNull(service);
            Assert.IsTrue(service is ActiveDirectoryAccountService);
        }

        [Test]
        public void create_db_directory_service_test() {
            ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"] = "FrameworkExtKit.Services.DirectoryServices.DB.DbDirectoryAccountService";
            ConfigurationManager.AppSettings["DirectoryAccountService.RootEntry"] = "DB://DBDirectoryService";
            IDirectoryAccountService service = DefaultDirectoryServiceFactory.GetDirectoryAccountService();
            Assert.IsNotNull(service);
            Assert.IsTrue(service is DbDirectoryAccountService);
        }

        [Test]
        public void create_invalid_directory_service_test() {
            ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"] = "";
            ConfigurationManager.AppSettings["DirectoryAccountService.RootEntry"] = "DB://DBDirectoryService";

            Assert.Catch<Exception>(() => {
                    IDirectoryAccountService service = DefaultDirectoryServiceFactory.GetDirectoryAccountService();
                },
                "DirectoryAccountService.ClassName is not found in the application or web config. \n Please add <add key=\"DirectoryAccountService.ClassName\" value=\"xyz\" /> to the config file."
            );
        }
    }
}
