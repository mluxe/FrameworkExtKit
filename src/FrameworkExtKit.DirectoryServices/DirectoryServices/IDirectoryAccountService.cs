using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public partial interface IDirectoryAccountService<T>: IDirectoryService<T> where T : DirectoryAccount {
        T FindByAlias(string alias);
        IEnumerable<T> FindTeamMembers(T user);
        T FindDirectManager(T user);
        IEnumerable<T> FindFunctionManagers(T user);
    }
}
