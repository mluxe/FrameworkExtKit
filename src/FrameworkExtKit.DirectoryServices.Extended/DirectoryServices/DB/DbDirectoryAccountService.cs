using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Configuration;
#if NETCORE
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
#endif

namespace FrameworkExtKit.Services.DirectoryServices.DB {

    /**
     *
     * The DBDirectoryService is one of the Directory Service family
     * It reads the LDAP records from a database dump of the Central LDAP server
     * If you query LDAP very frequently especially query for large amount of users 
     * you need to consider using this DB level directory service
     * 
     * A regular dump of the LDAP records into the db is required when you use DBDirectoryService
     * 
     * For the structure of the database table please refer to the DBDirectoryAccount.cs
     * 
     * To use the Directory Service, you can 
     * 1. add new configuration to the config file
     * <add key="DirectoryAccountService.ClassName" value="FrameworkExtKit.Services.DirectoryServices.DB.DbDirectoryService" />
     * 
     * 2. add db connection in appsettings
     * <add key="DirectoryAccountService.RootEntryPath" value="DB://{ConnectStringName}" />
     * 
     * 3. add db connection string
     * <add name="{ConnectStringName}" connectionString="data source=localhost;user=sa;password={password};initial catalog=LDAP;" providerName="System.Data.SqlClient" />
     * 
     * 4. then use this method to create a directory instance
     *      DirectoryAccountService.GetUserDirectoryInstance()
     * 
     * By: Yufei Liu <yliu@leyun.co.uk>
     * Date: 8th May, 2015 @ Gatwick, UK
     * 
     */

    internal class DbDirectoryAccountContext : DbDirectoryContext<DbDirectoryAccount> {
#if NET45
        public DbDirectoryAccountContext(string connectionName) : base(connectionName) {
        }
#endif

#if NETCORE
        public DbDirectoryAccountContext(DbContextOptions options):base(options) {

        }
#endif
    }

    public class DbDirectoryAccountService : DbDirectoryAccountService<DirectoryAccount>, IDirectoryAccountService {

        

        String connectionName;
#if NETCORE
        String connectionString;
#endif
        private DbDirectoryAccountContext dbContext;

        public DbDirectoryAccountService() {
#if NET45
            string url = ConfigurationManager.AppSettings["DirectoryAccountService.RootEntryPath"];
            if (String.IsNullOrEmpty(url)) {
                throw new KeyNotFoundException("The configuration key is not found in the config file, please add 'DirectoryAccountService.RootEntryPath' to the appSettings config.");
            }
            connectionName = url.Replace("DB://", "");
            dbContext = new DbDirectoryAccountContext(connectionName);
#endif
#if NETCORE
            var dir = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(dir)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            connectionName = configuration.GetValue<string>("DirectoryAccountService:connectionString");
            connectionString = configuration.GetConnectionString(connectionName);
            var optionsBuilder = new DbContextOptionsBuilder<DbDirectoryAccountContext>();
            optionsBuilder.UseSqlServer(connectionString);
            dbContext = new DbDirectoryAccountContext(optionsBuilder.Options);
#endif
        }

        public DbDirectoryAccountService(string connectionName): base(connectionName) {
            this.connectionName = connectionName;
#if NET45
            dbContext = new DbDirectoryAccountContext(connectionName);
#endif
#if NETCORE
            var dir = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(dir)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<DbDirectoryAccountContext>();
            connectionString = configuration.GetConnectionString(connectionName);
            optionsBuilder.UseSqlServer(connectionString);
            dbContext = new DbDirectoryAccountContext(optionsBuilder.Options);
#endif
        }

#if NETCORE
        public DbDirectoryAccountService(DbContextOptions options){
            dbContext = new DbDirectoryAccountContext(options);
        }
#endif


        public override IEnumerable<DirectoryAccount> Where(Expression<Func<DirectoryAccount, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().Where(predicate).ToList();
        }

        public override DirectoryAccount Single(Expression<Func<DirectoryAccount, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().Single(predicate);
        }

        public override DirectoryAccount SingleOrDefault(Expression<Func<DirectoryAccount, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().SingleOrDefault(predicate);
        }

        public override DirectoryAccount First(Expression<Func<DirectoryAccount, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().First(predicate);
        }

        public override DirectoryAccount FirstOrDefault(Expression<Func<DirectoryAccount, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().SingleOrDefault(predicate);
        }

        public override IEnumerable<DirectoryAccount> FindTeamMembers(DirectoryAccount user) {
            List<DbDirectoryAccount> records = dbContext.DirectoryEntities.AsNoTracking()
                                                .Where(a => a.ManagerDNString.Contains(user.DistinguishName))
                                                .ToList();
            return records;
        }


        public override void Close() {
#if NET45
            dbContext.Database.Connection.Close();
#endif
#if NETCORE
            dbContext.Database.CloseConnection();
#endif
        }

        public override void Open() {
            if (dbContext == null) {
#if NET45
                dbContext = new DbDirectoryAccountContext(connectionName);
#endif
#if NETCORE
            var optionsBuilder = new DbContextOptionsBuilder<DbDirectoryAccountContext>();
            optionsBuilder.UseSqlServer(connectionString);
            dbContext = new DbDirectoryAccountContext(optionsBuilder.Options);
#endif
            } else {
#if NET45
                dbContext.Database.Connection.Open();
#endif
#if NETCORE
                dbContext.Database.OpenConnectionAsync();
#endif
            }
        }
    }
}
