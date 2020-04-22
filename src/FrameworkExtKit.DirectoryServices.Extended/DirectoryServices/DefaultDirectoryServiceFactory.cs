using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

#if NETCORE
using Microsoft.Extensions.Configuration;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {
    /**
     * This is the base class of the DirectoryService Family
     * 
     * It hides all the internal implementations of the other directory service classes
     * and provides a unique API to access LDAP records hosted in different type of servers
     * including LDAP server, AD server and database servers
     * 
     * 
     * How to use it:
     * appSettings configuration on Web.conf 
     * LDAP service
     * <add key="DirectoryAccountService.RootEntryPath" value="LDAP://ldap.sample.com/o=sample,c=an" />
     * ActiveDirectory service
     * <add key="DirectoryAccountService.RootEntryPath" value="LDAP://GB0882DOM19.DIR.sample.com" />
     * DB directory service
     * <add key="DirectoryAccountService.RootEntryPath" value="DB://{ConnectStringName}" />
     * 
     * To use the Directory Service, you can 
     * 1. add new configuration to the config file
     * <add key="DirectoryAccountService.ClassName" value="{className}"
     * className is one of the following
     *      1) FrameworkExtKit.Services.DirectoryServices.LDAPDirectoryService
     *      2) FrameworkExtKit.Services.DirectoryServices.ActiveDirectoryService
     *      3) FrameworkExtKit.Services.DirectoryServices.DB.DbDirectoryService
     * 2. then use this method to create a directory instance
     *      DirectoryAccountService.GetUserDirectoryInstance()
     *      note: you still need to add the DirectoryAccountService.RootEntryPath parameter
     *          in the config file.
     *          
    * By: Yufei Liu <yliu@leyun.co.uk>
    * Date: 12th Nov, 2014 @ Gatwick, UK
     */
#if NET45
    public class DefaultDirectoryServiceFactory {

        public static IDirectoryDistributionListService GetDirectoryDistributionListService() {
            throw new NotImplementedException();
        }

        public static IDirectoryGroupService GetDirectoryGroupService() {
            throw new NotImplementedException();
        }

        public static IDirectoryRoleAccountService GetDirectoryRoleAccountService() {
            throw new NotImplementedException();
        }

        public static IDirectoryService<TDirectoryEntity> GetDirectoryService<TDirectoryEntity>() where TDirectoryEntity : DirectoryEntity {
            throw new NotImplementedException();
        }

//        private static object syncRoot = new object();
//        private static volatile IDirectoryAccountService instance;
        /** Static Methods **/
        public static IDirectoryAccountService GetDirectoryAccountService() {
            //DirectoryAccountService service;
            IDirectoryAccountService instance = null;
            if (instance == null) {

                //lock (syncRoot) {
                    string className = ConfigurationManager.AppSettings["DirectoryAccountService.ClassName"];
                    if (String.IsNullOrEmpty(className)) {
                        throw new SettingsPropertyNotFoundException("DirectoryAccountService.ClassName is not found in the application or web config. \n Please add <add key=\"DirectoryAccountService.ClassName\" value=\"xyz\" /> to the config file.");
                    }
                    Type type = Type.GetType(className);

                    if (type == null) {
                        throw new ConfigurationErrorsException("Invalid class type of " + className + " configured in AppSettings ");
                    }
                    instance = (IDirectoryAccountService)Activator.CreateInstance(type);
                //}
                
            } else {
                //CurrentService.Open();
            }

            return instance;
        }
    }
#endif

#if NETCORE
    public sealed class DefaultDirectoryServiceFactory : IDirectoryServiceFactory {

        private object syncRoot = new object();
        private IDirectoryAccountService instance;

        /** Static Methods **/
        public IDirectoryAccountService GetDirectoryAccountService() {
            //DirectoryAccountService service;
            if (instance == null) {
                lock (syncRoot) {
                    if (instance == null) {
                        var configuration = new ConfigurationBuilder()
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .Build();
                        string className = configuration.GetValue<string>("DirectoryAccountService:className");

                        if (String.IsNullOrEmpty(className)) {
                            throw new Exception("DirectoryAccountService.ClassName is not found in the appsettings.json config. \n Please add \"DirectoryAccountService\": { \"ClassName\": \"{class_name}\" } to the config file.");
                        }
                        Type type = Type.GetType(className);

                        if (type == null) {
                            throw new TypeLoadException("Invalid class type of " + className + " configured in AppSettings ");
                        }
                        instance = (IDirectoryAccountService)Activator.CreateInstance(type);
                    }

                }

            } else {
                //CurrentService.Open();
            }

            return instance;
        }

        public IDirectoryDistributionListService GetDirectoryDistributionListService() {
            throw new NotImplementedException();
        }

        public IDirectoryGroupService GetDirectoryGroupService() {
            throw new NotImplementedException();
        }

        public IDirectoryRoleAccountService GetDirectoryRoleAccountService() {
            throw new NotImplementedException();
        }

        public IDirectoryService<TDirectoryEntity> GetDirectoryService<TDirectoryEntity>() where TDirectoryEntity : DirectoryEntity {
            throw new NotImplementedException();
        }
    }
#endif
}
