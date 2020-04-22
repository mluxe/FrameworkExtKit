using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public partial interface IDirectoryDistributionListService<T>: IDirectoryService<T> where T : DirectoryDistributionList {
        // TDirectoryAccount FindOwner<TDirectoryAccount>(T list) where TDirectoryAccount : DirectoryAccount;
        // IEnumerable<TDirectoryAccount> FindProxies<TDirectoryAccount>(T list) where TDirectoryAccount : DirectoryAccount;
        IEnumerable<T> FindByOwner(string ownerIdentifier);
        IEnumerable<T> FindByProxy(string proxyIdentifier);
    }
}
