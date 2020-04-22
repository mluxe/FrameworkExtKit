using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif
#if NETCORE
using Novell.Directory.Ldap;
using FrameworkExtKit.Services.DirectoryServices;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {
    public class ActiveDirectoryAccount : DirectoryAccount{

        [DirectoryProperty("SAMAccountName")]
        public override string Alias { get; set; }

        [DirectoryProperty("title")]
        public override string JobTitle { get; set; }

        [DirectoryProperty("uidNumber")]
        public override long Id { get; set; }

        [DirectoryProperty("ipPhone")]
        public override string[] TelephoneNumbers { get; set; }

        [DirectoryProperty("distinguishedName")]
        public override string DistinguishName { get; set; }

        [DirectoryProperty("streetaddress")]
        public override string Street { get; set; }
    }
}
