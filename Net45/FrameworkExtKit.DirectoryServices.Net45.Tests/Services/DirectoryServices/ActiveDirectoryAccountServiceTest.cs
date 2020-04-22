using FrameworkExtKit.Services.DirectoryServices;
using NUnit.Framework;
using System;
using System.DirectoryServices;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {

    public partial class ActiveDirectoryAccountServiceTest{

        public ActiveDirectoryAccountServiceTest() {
            rootEntry = new DirectoryEntry("LDAP://DIR.company.com");
            service = new ActiveDirectoryAccountService(rootEntry);
        }

    }
}
