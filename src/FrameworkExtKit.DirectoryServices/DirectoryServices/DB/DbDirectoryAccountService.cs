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

    public class DbDirectoryAccountService<T> : DirectoryAccountService<T> where T : DirectoryAccount {


        String connectionName;
#if NETCORE
        String connectionString;
#endif
        DbDirectoryContext<T> dbContext;

        public DbDirectoryAccountService() {
#if NET45
            string url = ConfigurationManager.AppSettings["DirectoryAccountService.RootEntryPath"];
            if (String.IsNullOrEmpty(url)) {
                throw new KeyNotFoundException("The configuration key is not found in the config file, please add 'DirectoryAccountService.RootEntryPath' to the appSettings config.");
            }
            connectionName = url.Replace("DB://", "");
            dbContext = new DbDirectoryContext<T>(connectionName);
#endif
#if NETCORE
            var dir = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(dir)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            connectionName = configuration.GetValue<string>("DirectoryAccountService:connectionString");
            connectionString = configuration.GetConnectionString(connectionName);
            var optionsBuilder = new DbContextOptionsBuilder<DbDirectoryContext<T>>();
            optionsBuilder.UseSqlServer(connectionString);
            dbContext = new DbDirectoryContext<T>(optionsBuilder.Options);
#endif
        }

        public DbDirectoryAccountService(string connectionName) {
            this.connectionName = connectionName;
#if NET45
            dbContext = new DbDirectoryContext<T>(connectionName);
#endif
#if NETCORE
            var dir = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(dir)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<DbDirectoryContext<T>>();
            connectionString = configuration.GetConnectionString(connectionName);
            optionsBuilder.UseSqlServer(connectionString);
            dbContext = new DbDirectoryContext<T>(optionsBuilder.Options);
#endif
        }

#if NETCORE
        public DbDirectoryAccountService(DbContextOptions options){
            dbContext = new DbDirectoryContext<T>(options);
        }
#endif

        public override void Close() {
            if (dbContext == null) {
                return;
            }
            lock (dbContext) {
#if NET45
                dbContext.Database.Connection.Close();
#endif
#if NETCORE
                dbContext.Database.CloseConnection();
#endif
                dbContext = null;
            }

        }

        private Object lockObject = new Object();
        public override void Open() {

            if (dbContext == null) {
                lock (lockObject) {
                    DbDirectoryContext<T> context;
#if NET45
                    if (dbContext == null) {
                        context = new DbDirectoryContext<T>(connectionName);
                        context.Database.Connection.Open();
                        dbContext = context;
                    }

#endif
#if NETCORE
                    if (dbContext == null) {
                        var optionsBuilder = new DbContextOptionsBuilder<DbDirectoryContext<T>>();
                        optionsBuilder.UseSqlServer(connectionString);
                        context = new DbDirectoryContext<T>(optionsBuilder.Options);
                        context.Database.OpenConnectionAsync();
                        dbContext = context;
                    }
#endif
                }
            }

        }

        public override IEnumerable<T> Where(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().Where(predicate).ToList();
        }

        public override T Single(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().Single(predicate);
        }

        public override T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().SingleOrDefault(predicate);
        }

        public override T First(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().First(predicate);
        }

        public override T FirstOrDefault(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.AsNoTracking().SingleOrDefault(predicate);
        }

    }
}
