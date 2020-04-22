using System;
using System.Collections.Generic;
using System.Text;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {
    public partial class DirectoryGroup : DirectoryEntity {
        public virtual string[] Proxies { get; set; }
        [DirectoryProperty("uniquemember")]
        public virtual string[] Members { get; set; }
        [DirectoryProperty("proxy")]
        public virtual string Proxy { get; set; }
        [DirectoryProperty("owner")]
        public virtual string Owner { get; set; }
        public virtual int OwnerDirectoryId {
            get {
                string dn = this.Owner;
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

        public DirectoryGroup() {
            this.ObjectClass = "group";
        }
    }
}
