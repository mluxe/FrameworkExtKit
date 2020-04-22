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
    /// <summary>
    /// The LdapDirectoryService is one of the Directory Service family
    ///
    /// It connects to the LDAP server and convert all data into the LdapDirectoryAccount object
    ///
    /// To use the Directory Service, you can 
    /// 1. add new configuration to the config file
    /// &lt;add key="DirectoryAccountService.ClassName" value="FrameworkExtKit.Services.DirectoryServic.LdapDirectoryService" /&gt;
    ///
    /// 2. add db connection in appsettings
    /// <add key="DirectoryAccountService.RootEntryPath" value="LDAP://ldap.company.com/o=company,c=an" />
    ///
    /// 3. then use this method to create a directory instance
    ///      DirectoryAccountService.GetUserDirectoryInstance()
    ///
    /// By: Yufei Liu <yliu@leyun.co.uk>
    /// Date: 12th Nov, 2014 @ Gatwick, UK
    /// </summary>
    public class LdapDirectoryAccountService : LdapDirectoryAccountService<DirectoryAccount>, IDirectoryAccountService {

#if NET45
        public LdapDirectoryAccountService():base() {
        }

        public LdapDirectoryAccountService(DirectoryEntry rootEntry):base(rootEntry) {
        }
#endif
#if NETCORE
        public LdapDirectoryAccountService():base() {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
            var configuration_section = this.GetConfiguration(configuration);
            LdapSettings settings = new LdapSettings();
            configuration_section.Bind(settings);
            this.ldapDirectoryService.LdapSettings = settings;
        }

        public LdapDirectoryAccountService(IConfiguration configuration):base(configuration) {
        }

        public LdapDirectoryAccountService(IOptions<LdapSettings> ldapSettingsOptions): base(ldapSettingsOptions) {
        }
        public LdapDirectoryAccountService(LdapSettings ldapSettings): base(ldapSettings) {
        }

        protected IConfiguration GetConfiguration(IConfiguration configuration) {
            IConfigurationSection section = configuration.GetSection("DirectoryServices:AccountService:LdapSettings");

            if(section.Value == null) {
                section = configuration.GetSection("LdapSettings");
            }
            return section;
        }

#endif

    }

}
