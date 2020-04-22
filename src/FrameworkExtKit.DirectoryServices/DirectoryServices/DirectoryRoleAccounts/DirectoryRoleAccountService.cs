using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public abstract partial class DirectoryRoleAccountService<T> : DirectoryService<T>, IDirectoryRoleAccountService<T> where T : DirectoryRoleAccount {

        public override string ObjectClass { get; set; } = "role";

        public virtual IEnumerable<T> FindByOwner(string managerIdentifier) {
            return this.Where(entity => entity.Manager == managerIdentifier);
        }

        public virtual IEnumerable<T> FindByManager(string managerIdentifier) {
            return this.Where(entity => entity.Manager == managerIdentifier);
        }


        public virtual IEnumerable<T> FindByProxy(string proxyIdentifier) {
            return this.Where(entity => entity.Proxy.Contains(proxyIdentifier));
        }
    }
}
