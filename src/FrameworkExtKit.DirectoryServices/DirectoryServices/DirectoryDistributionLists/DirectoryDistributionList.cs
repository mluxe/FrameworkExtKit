using System;
using System.Collections.Generic;
using System.Text;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {
    public partial class DirectoryDistributionList : DirectoryEntity {
        [DirectoryProperty("mail")]
        public virtual string Mail { get; set; }
        [DirectoryProperty("subscriptions")]
        public virtual string[] Subscriptions { get; set; }
        [DirectoryProperty("proxy")]
        public virtual string Proxy { get; set; }
        [DirectoryProperty("manager")]
        public virtual string ManagerDn { get; set; }
        [DirectoryProperty("manager")]
        public virtual int ManagerDirectoryID {
            get {
                string dn = this.ManagerDn;
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
    }
}
