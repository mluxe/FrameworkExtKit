using System;
using System.Collections.Generic;
using System.Linq;
#if NET45
using System.DirectoryServices;
#endif
#if NETCORE
using Microsoft.Extensions.Options;
using FrameworkExtKit.Services.DirectoryServices.Settings;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {
    /**
     *
     * The ActiveDirectoryService is one of the Directory Service family
     * 
     * It connects to the LDAP server and convert all data into the LDAPDirectoryAccount object
     * 
     * To use the Directory Service, you can 
     * 1. add new configuration to the config file
     * <add key="DirectoryAccountService.ClassName" value="FrameworkExtKit.Services.DirectoryServices.ActiveDirectoryService"
     * 
     * 2. add db connection in appsettings
     * <add key="DirectoryAccountService.RootEntryPath" value="LDAP://GB0882DOM19.DIR.sample.com" />
     * 
     * 3. then use this method to create a directory instance
     *      DirectoryAccountService.GetUserDirectoryInstance()
     * 
     * By: Yufei Liu <feilfly@gmail.com>
     * Date: 12th Nov, 2014 @ Gatwick, UK
     * 
     */

    public class ActiveDirectoryAccountService : LdapDirectoryAccountService<DirectoryAccount>, IDirectoryAccountService {


        public ActiveDirectoryAccountService() {
            DirectoryAccountType = typeof(ActiveDirectoryAccount);
        }

#if NET45
        public ActiveDirectoryAccountService(DirectoryEntry rootEntry) : base(rootEntry) {
            DirectoryAccountType = typeof(ActiveDirectoryAccount);
        }
#endif
#if NETCORE
        public ActiveDirectoryAccountService(IOptions<LdapSettings> ldapSettingsOptions) : base(ldapSettingsOptions) {
            DirectoryAccountType = typeof(ActiveDirectoryAccount);
        }
        public ActiveDirectoryAccountService(LdapSettings ldapSettings) : base(ldapSettings) {
            DirectoryAccountType = typeof(ActiveDirectoryAccount);
        }
#endif

    }
}
