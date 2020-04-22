using FrameworkExtKit.Services.DirectoryServices.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FrameworkExtKit.Services.DirectoryServices.Exceptions;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif
#if NETCORE
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using Microsoft.Extensions.Configuration;
using FrameworkExtKit.Services.DirectoryServices.Settings;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {

    public abstract partial class DirectoryAccountService<TDirectoryAccount> {

        public virtual TDirectoryAccount FindByGIN(string gin) {
            return this.SingleOrDefault(account => account.GIN == gin);
        }

        [Obsolete]
        public virtual IEnumerable<TDirectoryAccount> SearchUser(string key) {
            return this.Where(account => (account.ObjectClass == "Person") &&
                                (account.GivenName.Contains(key) ||
                                account.SurName.Contains(key) ||
                                account.DisplayName.Contains(key) ||
                                account.GIN == key || account.Alias == key));
        }

        [Obsolete]
        public virtual IEnumerable<TDirectoryAccount> SearchDistributionList(string key) {
            return this.Where(account => (account.ObjectClass == "list") &&
                    (account.GivenName.Contains(key) ||
                    account.SurName.Contains(key) ||
                    account.DisplayName.Contains(key) ||
                    account.GIN == key || account.Alias == key));
        }

        [Obsolete]
        public virtual IEnumerable<TDirectoryAccount> SearchServiceAccount(string key) {
            return this.Where(account => (account.ObjectClass == "role") &&
                    (account.GivenName.Contains(key) ||
                    account.SurName.Contains(key) ||
                    account.DisplayName.Contains(key) ||
                    account.GIN == key || account.Alias == key));
        }
    }

}
