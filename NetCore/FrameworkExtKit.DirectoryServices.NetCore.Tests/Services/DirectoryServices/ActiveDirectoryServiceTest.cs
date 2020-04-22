using FrameworkExtKit.Services.DirectoryServices;
using FrameworkExtKit.Services.DirectoryServices.Settings;
using NUnit.Framework;
using System;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {


    //[Ignore("ignore the tests because AD requires ldap bind before we can search, " +
    //"we do not have a useful username and password yet")]
    public partial class ActiveDirectoryAccountServiceTest : DirectoryAccountServiceTest {

        public ActiveDirectoryAccountServiceTest() {
            var settings = new LdapSettings();
            settings.ServerName = "dir.company.com";
            settings.SearchBase = "o=company,c=an";
            //settings.Credentials.DomainUserName = "dir\\lyufei";
            //settings.Credentials.Password = "";
            service = new ActiveDirectoryAccountService(settings);
        }
    }
}
