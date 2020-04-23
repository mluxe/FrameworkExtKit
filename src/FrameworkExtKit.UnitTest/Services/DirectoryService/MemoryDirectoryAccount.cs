using FrameworkExtKit.Services.DirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 *
 * The MemoryDirectoryAccount is a sub class of DirectoryAccount to work with the MemoryDirectoryService
 * 
 * The MemoryDirectoryService is developed as a utility to help with the unit testing
 * so we do not have to change the unit testing code when LDAP reporting structure changes
 * 
 * By: Yufei Liu <feilfly@gmail.com>
 * Date: 11th June, 2015 @ Gatwick, UK
 * 
 */
namespace FrameworkExtKit.UnitTest.Services.DirectoryService {
    public partial class MemoryDirectoryAccount : DirectoryAccount {

        public MemoryDirectoryAccount() {
            this.Subscriptions = new string[0];
        }
    }
}
