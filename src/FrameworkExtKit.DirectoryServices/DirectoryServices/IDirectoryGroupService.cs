using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {


    public partial interface IDirectoryGroupService<T>: IDirectoryService<T> where T : DirectoryGroup {
        // TDirectoryAccount FindOwner<TDirectoryAccount>(T group) where TDirectoryAccount : DirectoryAccount;
        // TDirectoryAccount FindProxies<TDirectoryAccount>(T group) where TDirectoryAccount : DirectoryAccount;
        IEnumerable<T> FindByOwner(string ownerIdentifier);
        IEnumerable<T> FindByProxy(string proxyIdentifier);
    }
}
