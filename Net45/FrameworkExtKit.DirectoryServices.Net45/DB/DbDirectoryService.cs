using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Configuration;

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
    
    public partial class DbDirectoryService<T> {

        public DbDirectoryService() {

            string url = ConfigurationManager.AppSettings["DirectoryAccountService.RootEntryPath"];
            if (String.IsNullOrEmpty(url)) {
                throw new KeyNotFoundException("The configuration key is not found in the config file, please add 'DirectoryAccountService.RootEntryPath' to the appSettings config.");
            }
            connectionName = url.Replace("DB://", "");
            dbContext = new DbDirectoryContext<T>(connectionName);

        }

        public DbDirectoryService(string connectionName) {
            this.connectionName = connectionName;
            dbContext = new DbDirectoryContext<T>(connectionName);

        }

        public override void Close() {
            if(dbContext == null) {
                return;
            }
            lock (dbContext) {
                dbContext.Database.Connection.Close();
                dbContext = null;
            }

        }

        private Object lockObject = new Object();
        public override void Open() {

            if (dbContext == null) {
                lock (lockObject) {
                    DbDirectoryContext<T> context;
                    if (dbContext == null) {
                        context = new DbDirectoryContext<T>(connectionName);
                        context.Database.Connection.Open();
                        dbContext = context;
                    }
                }
            }

        }
    }
}
