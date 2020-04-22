using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public partial interface IDirectoryRoleAccountService<T>: IDirectoryService<T> where T : DirectoryRoleAccount {
        // TDirectoryAccount FindOwner<TDirectoryAccount>(T account) where TDirectoryAccount : DirectoryAccount;
        // TDirectoryAccount FindProxies<TDirectoryAccount>(T account) where TDirectoryAccount : DirectoryAccount;
        IEnumerable<T> FindByOwner(string ownerIdentifier);
        IEnumerable<T> FindByProxy(string proxyIdentifier);
    }
}
