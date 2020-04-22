using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices.Settings {
    public class LdapCredentials {
//        public string Method { get; set; }
        public string BindDn { get; set; }
        public string BindCredential { get; set; }
    }

    public class LdapSettings {
        public string ServerName { get; set; }
        public int ServerPort { get; set; } = 389;
        public bool UseSSL { get; set; } = false;
        public string SearchBase { get; set; }
        public string ContainerName { get; set; }
        //public string DomainName { get; set; }
        //public string DomainDistinguishedName { get; set; }
        public LdapCredentials Bind { get; set; } = new LdapCredentials();
    }
}
