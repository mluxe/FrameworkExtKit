using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {
    public interface IDirectoryAccountService : IDirectoryAccountService<DirectoryAccount> {
    }

    public partial interface IDirectoryAccountService<T> {

        T FindByGIN(string gin);

        [Obsolete("retired method, it will be replaced by IDirectoryAccountService.Search(string key)")]
        IEnumerable<T> SearchUser(string key);
        [Obsolete("retired method, it will be replaced by IDirectoryRoleAccountService.Search(string key)")]
        IEnumerable<T> SearchServiceAccount(string key);
        [Obsolete("retired method, it will be replaced by IDirectoryDistributionListService.Search(string key)")]
        IEnumerable<T> SearchDistributionList(string key);

    }
}