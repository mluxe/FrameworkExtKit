using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public abstract partial class DirectoryDistributionListService<T> : DirectoryService<T>, IDirectoryDistributionListService<T> where T : DirectoryDistributionList {

        public override string ObjectClass { get; set; } = "list";

        public virtual IEnumerable<T> FindByManager(string managerIdentifier) {
            return this.Where(entity => entity.ManagerDn == managerIdentifier);
        }

        public virtual IEnumerable<T> FindByOwner(string managerIdentifier) {
            return this.Where(entity => entity.ManagerDn == managerIdentifier);
        }
        public virtual IEnumerable<T> FindByProxy(string proxyIdentifier) {
            return this.Where(entity => entity.Proxy.Contains(proxyIdentifier));
        }
    }
}
