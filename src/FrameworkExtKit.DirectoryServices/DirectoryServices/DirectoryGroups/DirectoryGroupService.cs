using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public abstract partial class DirectoryGroupService<T> : DirectoryService<T>, IDirectoryGroupService<T> where T : DirectoryGroup {

        public override string ObjectClass { get; set; } = "group";

        public virtual IEnumerable<T> FindByOwner(string managerIdentifier) {
            return this.Where(entity => entity.Owner == managerIdentifier);
        }
        public virtual IEnumerable<T> FindByProxy(string proxyIdentifier) {
            return this.Where(entity => entity.Proxy.Contains(proxyIdentifier));
        }
    }
}
