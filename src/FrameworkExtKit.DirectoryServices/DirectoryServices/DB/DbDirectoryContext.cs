using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NET45
using System.Data.Entity;
#endif 

#if NETCORE
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
#endif

namespace FrameworkExtKit.Services.DirectoryServices.DB {
    /**
     * 
     * This is the Database Context for DBDirectory Service
     * 
     * By: Yufei Liu <feilfly@gmail.com>
     * Date: 8th May, 2015 @ Gatwick, UK
     * 
     */
    internal class DbDirectoryContext<T> : DbContext where T:DirectoryEntity{

#if NET45
        public DbDirectoryContext(string connectionName) : base("name="+connectionName){
            Database.SetInitializer<DbDirectoryContext<T>>(null);
        }
#endif

#if NETCORE
        public DbDirectoryContext(DbContextOptions options):base(options) {

        }
#endif

        public DbSet<T> DirectoryEntities { get; set; }
    }
    /*
    internal class DbDirectoryAccountContext<T> : DbDirectoryContext<T> where T : DirectoryAccount {
#if NET45
        public DbDirectoryAccountContext(string connectionName) : base(connectionName) {
        }
#endif

#if NETCORE
        public DbDirectoryAccountContext(DbContextOptions options):base(options) {

        }
#endif
    }
    */
}
