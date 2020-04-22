using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public interface IDirectoryRoleAccountService : IDirectoryRoleAccountService<DirectoryRoleAccount> {
    }

    public partial interface IDirectoryRoleAccountService<T> {
        T FindByAlias(string alias);
        IEnumerable<T> FindByManager(string manager);
    }
}
