using System;
using System.Collections.Generic;
using System.Text;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {
    public partial class DirectoryRoleAccount : DirectoryEntity {
        [DirectoryProperty("Mail")]
        public virtual string Mail { get; set; }
        [DirectoryProperty("street")]
        public virtual string Street { get; set; }
        [DirectoryProperty("st")]
        public virtual string State { get; set; }
        [DirectoryProperty("postalcode")]
        public virtual string PostalCode { get; set; }
        [DirectoryProperty("manager")]
        public virtual string Manager { get; set; }

        public virtual int ManagerDirectoryId {
            get {
                string dn = this.Manager;
                int id = 0;
                if (!String.IsNullOrEmpty(dn)) {
                    dn = dn.Substring(dn.IndexOf('=') + 1);
                    dn = dn.Substring(0, dn.IndexOf(','));
                    dn = dn.Substring(dn.LastIndexOf(' ') + 1);
                    id = Convert.ToInt32(dn);
                }

                return id;
            }
        }

        public virtual string[] Proxies { get; set; }
        [DirectoryProperty("proxy")]
        public virtual string Proxy { get; set; }
        [DirectoryProperty("description")]
        public virtual string Description { get; set; }
    }
}
