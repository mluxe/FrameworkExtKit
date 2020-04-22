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

    public abstract partial class DirectoryAccountService<TDirectoryAccount> : DirectoryService<TDirectoryAccount>, IDirectoryAccountService<TDirectoryAccount> where TDirectoryAccount : DirectoryAccount {

        public override string ObjectClass { get; set; } = "Person";


        public virtual TDirectoryAccount FindByAlias(string alias) {
            return this.SingleOrDefault(account => account.Alias == alias);
        }

        public override TDirectoryAccount FindByIdentifier(string identifier) {
            return this.SingleOrDefault(account => account.Alias == identifier ||
                                                    account.GIN == identifier ||
                                                    //account.DistinguishName == identifier ||
                                                    account.UniqueId == identifier || account.Id.ToString() == identifier);
        }

        public override IEnumerable<TDirectoryAccount> FindByName(string name) {
            return this.Where(account => account.GivenName.Contains(name) ||
                                            account.SurName.Contains(name) ||
                                            account.DisplayName.Contains(name));
        }
        public override IEnumerable<TDirectoryAccount> Search(string key) {
            return this.Where(account => account.GivenName.Contains(key) ||
                                account.SurName.Contains(key) ||
                                account.DisplayName.Contains(key) ||
                                account.GIN == key || account.Alias == key);
        }


        public virtual IEnumerable<TDirectoryAccount> FindTeamMembers(TDirectoryAccount user) {
            if (user == null) {
                throw new ArgumentNullException("user");
            }
            // string query = String.Format("(manager={0})", user.DistinguishName);

            //return this.Where(u => u.DirectManager.Contains(user.DistinguishName));
            return this.Where(u => u.DirectManager == user.DistinguishName);
        }

        public virtual TDirectoryAccount FindDirectManager(TDirectoryAccount user) {
            return this.SingleOrDefault(account => account.Id == user.DirectManagerID);
        }

        public virtual IEnumerable<TDirectoryAccount> FindFunctionManagers(TDirectoryAccount user) {
            long[] ManagerIDs = user.ManagerDirectoryIDs;
            long[] functionManagerIds = new long[0];
            string managerDNFilter = String.Empty;

            if (ManagerIDs.Length > 1) {
                functionManagerIds = new long[ManagerIDs.Length - 1];
                for (int i = 1; i < ManagerIDs.Length; i++) {
                    functionManagerIds[i - 1] = ManagerIDs[i];
                }
            }
            // todo: remove direct manager from the array
            return this.Where(account => functionManagerIds.Contains(account.Id));
        }
        //public virtual TDirectoryAccount FindByUniqueId(string uniqueId) {
        //    return this.SingleOrDefault(account => account.UniqueId == uniqueId);
        //}

    }

}
