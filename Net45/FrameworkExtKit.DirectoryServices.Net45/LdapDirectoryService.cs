using FrameworkExtKit.Services.DirectoryServices.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FrameworkExtKit.Services.DirectoryServices.Exceptions;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;


namespace FrameworkExtKit.Services.DirectoryServices {


    public partial class LdapDirectoryService<T> {
        public LdapDirectoryService() {
        }

        public LdapDirectoryService(DirectoryEntry rootEntry) : base(rootEntry) {
        }
    }

    public partial class LdapDirectoryService { 

        public string Url { get; protected set; }
        public DirectoryEntry RootEntry { get; protected set; }

        private DirectorySearcher _directorySearcher;
        private DirectorySearcher directorySearcher {
            get {

                if(RootEntry == null) {
                    throw new Exception("RootEntry is null, please override the Find<T>(Type, Expression) method if you are not using LDAP search");
                }
                if (_directorySearcher != null) {
                    return _directorySearcher;
                }
                var searcher = new DirectorySearcher(RootEntry);
                searcher.ClientTimeout = Timeout;

                _directorySearcher = searcher;
                return _directorySearcher;
                /*
                if (_directorySearcher == null) {
                    _directorySearcher = new DirectorySearcher(RootEntry);
                    _directorySearcher.ClientTimeout = Timeout;
                }
                return _directorySearcher;
                */
            }
        }

        public LdapDirectoryService() {
            string url = ConfigurationManager.AppSettings["DirectoryAccountService.RootEntryPath"];
            if (String.IsNullOrEmpty(url)) {
                throw new KeyNotFoundException("The configuration key is not found in the config file, please add 'DirectoryAccountService.RootEntryPath' to the appSettings config.");
            }

            // this.Timeout = TimeSpan.FromSeconds(10);
            this.Url = url;
        }

        public LdapDirectoryService(DirectoryEntry rootEntry) {
            // this.Timeout = TimeSpan.FromSeconds(10);
            this.Url = rootEntry.Path;
            this.RootEntry = rootEntry;
        }

        private IEnumerable<T> Find<T>(string ldap_filter) where T : DirectoryEntity {
            return this.Find<T>(typeof(T), ldap_filter);
        }

        private IEnumerable<T> Find<T>(Type type, string ldap_filter, string objectClass=null) where T : DirectoryEntity { 
            if(!typeof(T).IsAssignableFrom(type)) {
                throw new ArgumentException($"{type.FullName} is not subclass of {typeof(T).FullName}");
            }

            if (String.IsNullOrEmpty(ldap_filter)) {
                throw new ArgumentException("LDAP query must not be empty");
            }

            if (this.RootEntry == null) {
                this.Open();
            }

            IList<T> users = new List<T>();

            var properties = type.GetProperties();
            DirectorySearcher searcher = this.directorySearcher;
            searcher.Filter = ldap_filter;

            var _objectClass = "*";

            if (!String.IsNullOrEmpty(this.ObjectClass)) {
                _objectClass = this.ObjectClass;
            }

            if (!String.IsNullOrEmpty(objectClass)) {
                _objectClass = objectClass;
            }

            if (!String.IsNullOrEmpty(_objectClass)) {
                searcher.Filter = $"(&(objectclass={this.ObjectClass}){searcher.Filter})";
            }
            

            searcher.SizeLimit = this.SizeLimit;

            var results = searcher.FindAll();
            foreach (SearchResult result in results) {
                //var account = (TDirectoryAccount)DirectoryAccountType.Assembly.CreateInstance(DirectoryAccountType.FullName, result);
                var account = Activator.CreateInstance(type) as T;
                account.SearchResult = result;

                string dn = result.Path;

                // typical Path is
                // LDAP://my.ldap.server.com:39/CN=a,CN=b,OU=c
                // we want to grab the part after the third '/'
                int idx = dn.IndexOf('/', 7);
                if (idx >= 0 && dn.Length > idx + 1) {
                    dn = dn.Substring(idx + 1);
                }
                account.DistinguishName = dn;

                foreach (var property in properties) {
                    if(property.SetMethod != null) {
                        var attr = (DirectoryPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(DirectoryPropertyAttribute), false);

                        if (attr != null) {
                            if (property.PropertyType.IsArray) {
                                var values = this.GetValues(result, attr.SchemaAttributeName);
                                if (property.PropertyType == typeof(int[])) {
                                    var int_vals = new int[values.Length];
                                    for (var i = 0; i < values.Length; i++) {
                                        Int32.TryParse(values[i], out int_vals[i]);
                                    }
                                    property.SetValue(account, int_vals);
                                } else if (property.PropertyType == typeof(long)) {
                                    var lng_vals = new long[values.Length];
                                    for (var i = 0; i < values.Length; i++) {
                                        Int64.TryParse(values[i], out lng_vals[i]);
                                    }
                                    property.SetValue(account, lng_vals);
                                } else {
                                    property.SetValue(account, values);
                                }

                            } else {
                                var value = this.GetValue(result, attr.SchemaAttributeName);
                                if (property.PropertyType == typeof(int)) {
                                    int int_val = 0;
                                    Int32.TryParse(value, out int_val);
                                    property.SetValue(account, int_val);
                                } else if (property.PropertyType == typeof(long)) {
                                    long lng_val = 0;
                                    Int64.TryParse(value, out lng_val);
                                    property.SetValue(account, lng_val);
                                } else {
                                    property.SetValue(account, value);
                                }
                            }
                        }
                    }
                }

                users.Add(account);
            }
            return users;
        }

        public virtual void Close() {
            if (this.RootEntry != null) {
                this.RootEntry.Close();
                this.RootEntry = null;
            }
        }

        public virtual void Open() {
            if (this.RootEntry == null) {
                DirectoryEntry rootEntry = new DirectoryEntry(this.Url);
                rootEntry.AuthenticationType = AuthenticationTypes.None;
                this.RootEntry = rootEntry;
            }
        }
        public virtual void Dispose() {
            if (this.RootEntry != null) {
                this.RootEntry.Dispose();
            }
        }

        protected string[] GetValues(SearchResult searchResult, string name) {
            var properties = searchResult.Properties[name];
            var len = properties.Count;
            string[] results = new String[len];
            for (var i = 0; i < len; i++) {
                var propertyValue = properties[i];

                if (propertyValue is byte[]) {
                    byte[] bytes = (byte[])propertyValue;
                    results[i] = System.Text.Encoding.UTF8.GetString(bytes);
                } else {
                    results[i] = propertyValue.ToString();
                }
            }
            return results;
        }

        protected string GetValue(SearchResult searchResult, string name, int pos = -1) {
            string result = String.Empty;

            if (searchResult.Properties[name].Count > 0) {
                var properties = searchResult.Properties[name];
                if(pos < 0) {
                    pos = properties.Count + pos;
                }
                pos = Math.Max(0, pos);
                var propertyValue = properties[pos];

                if (propertyValue is byte[]) {
                    byte[] bytes = (byte[])propertyValue;
                    result = System.Text.Encoding.UTF8.GetString(bytes);
                } else {
                    result = propertyValue.ToString();
                }
            }
            return result;
        }

    }

}
