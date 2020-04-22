﻿using FrameworkExtKit.Services.DirectoryServices.Linq;
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

    public class LdapDirectoryAccountService<T> : DirectoryAccountService<T> where T : DirectoryAccount {

        //private DirectorySearcher _directorySearcher;
        internal protected LdapDirectoryService ldapDirectoryService;

        internal protected Type DirectoryAccountType = typeof(T);

#if NET45
        public LdapDirectoryAccountService() {
            // DirectoryAccountType = typeof(DirectoryAccount);
            this.ldapDirectoryService = new LdapDirectoryService();
            this.ldapDirectoryService.ObjectClass = this.ObjectClass;
        }

        public LdapDirectoryAccountService(DirectoryEntry rootEntry) {
            // DirectoryAccountType = typeof(DirectoryAccount);
            this.ldapDirectoryService = new LdapDirectoryService(rootEntry);
            this.ldapDirectoryService.ObjectClass = this.ObjectClass;
        }
#endif
#if NETCORE
        // LdapSettings LdapSettings;

        public LdapDirectoryAccountService() {
            this.ldapDirectoryService = new LdapDirectoryService();
            this.ldapDirectoryService.ObjectClass = this.ObjectClass;
        }

        public LdapDirectoryAccountService(IConfiguration configuration) { 
            this.ldapDirectoryService = new LdapDirectoryService(configuration);
            this.ldapDirectoryService.ObjectClass = this.ObjectClass;
        }

        public LdapDirectoryAccountService(IOptions<LdapSettings> ldapSettingsOptions) {
            this.ldapDirectoryService = new LdapDirectoryService(ldapSettingsOptions);

            this.ldapDirectoryService.ObjectClass = this.ObjectClass;
        }

        public LdapDirectoryAccountService(LdapSettings ldapSettings) {
            this.ldapDirectoryService = new LdapDirectoryService(ldapSettings);
            this.ldapDirectoryService.ObjectClass = this.ObjectClass;
        }

#endif

        public override IEnumerable<T> Where(Expression<Func<T, bool>> predicate) {
            return this.ldapDirectoryService.Where<T>(DirectoryAccountType, predicate);
        }

        public override T Single(Expression<Func<T, bool>> predicate) {
            return this.ldapDirectoryService.Single(DirectoryAccountType, predicate);
        }

        public override T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            return this.ldapDirectoryService.SingleOrDefault(DirectoryAccountType, predicate);
        }

        public override T First(Expression<Func<T, bool>> predicate) {
            return this.ldapDirectoryService.First(DirectoryAccountType, predicate);
        }

        public override T FirstOrDefault(Expression<Func<T, bool>> predicate) {
            return this.ldapDirectoryService.FirstOrDefault(DirectoryAccountType, predicate);
        }

        public override void Close() {
            this.ldapDirectoryService.Close();
        }

        public override void Open() {
            this.ldapDirectoryService.Open();
        }
    }

}
