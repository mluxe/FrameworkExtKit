using FrameworkExtKit.Services.DirectoryServices;
using FrameworkExtKit.UnitTest.Services.DirectoryServices.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace FrameworkExtKit.UnitTest.Services.DirectoryServices {

    /**
     *
     * The MemoryDirectoryService is developed as a utility to help with the unit testing
     * so we do not have to change the unit testing code when LDAP reporting structure changes
     * 
     * To use the Directory Service, you can 
     * 1. add new configuration to the config file
     * <add key="DirectoryAccountService.ClassName" value="FrameworkExtKit.Tests.Services.DirectoryService.MemoryDirectoryService, FrameworkExtKit.UnitTest" />
     * 
     * 2. then use this method to create a directory instance
     *      DirectoryAccountService.GetUserDirectoryInstance()
     * 
     * By: Yufei Liu <feilfly@gmail.com>
     * Date: 11th June, 2015 @ Gatwick, UK
     * 
     */
    public class MemoryDirectoryAccountService : MemoryDirectoryAccountService<DirectoryAccount>, IDirectoryAccountService {

        public MemoryDirectoryAccountService() : base(MemoryDirectoryAccounts.ToList()) {

        }

        public MemoryDirectoryAccountService(IEnumerable<DirectoryAccount> data) : base(data) {

        }
    }

}
