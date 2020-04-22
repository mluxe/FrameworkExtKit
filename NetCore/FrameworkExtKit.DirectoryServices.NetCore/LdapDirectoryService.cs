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
using System.Reflection;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {

    public partial class LdapDirectoryService<T> {
        public LdapDirectoryService() {
        }

        public LdapDirectoryService(IConfiguration configuration): base(configuration) {
        }

        public LdapDirectoryService(IOptions<LdapSettings> ldapSettingsOptions) : base(ldapSettingsOptions) {
        }

        public LdapDirectoryService(LdapSettings ldapSettingsOptions) : base(ldapSettingsOptions) {
        }
    }


    public partial class LdapDirectoryService { 

        protected string _searchBase;
        protected LdapSettings _ldapSettings = new LdapSettings();
        private LdapConnection _ldapConnection;

        

        public LdapDirectoryService() {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            this.ReadConfiguration(configuration);
        }
        public LdapDirectoryService(IConfiguration configuration) {
            this.ReadConfiguration(configuration);
        }

        public LdapDirectoryService(IOptions<LdapSettings> ldapSettingsOptions) {
            this._ldapSettings = ldapSettingsOptions.Value;
            this._searchBase = this._ldapSettings.SearchBase;
        }

        public LdapDirectoryService(LdapSettings ldapSettings) { 
            this._ldapSettings = ldapSettings;
            this._searchBase = this._ldapSettings.SearchBase;
        }

        public LdapSettings LdapSettings { get => this._ldapSettings; set { this._ldapSettings = value; this._searchBase = this._ldapSettings.SearchBase; } }

        protected virtual void ReadConfiguration(IConfiguration configuration) {
            var section = configuration.GetSection("LdapSettings");

            if(section == null) {
                configuration.Bind(this._ldapSettings);
            } else {
                section.Bind(this._ldapSettings);
            }
            
            this._searchBase = this._ldapSettings.SearchBase;
        }

        public virtual IEnumerable<T> Find<T>(string ldap_filter) where T : DirectoryEntity {
            return this.Find<T>(typeof(T), ldap_filter);
        }

        public virtual IEnumerable<T> Find<T>(Type type, string ldap_filter) where T : DirectoryEntity {

            if (!typeof(T).IsAssignableFrom(type)) {
                throw new ArgumentException($"{type.FullName} is not subclass of {typeof(T).FullName}");
            }

            // if (String.IsNullOrEmpty(this.ObjectClass)) {
                // throw new ArgumentException($"object class must not be empty to make sure ldap search is efficient, set the value to '*' if you want to search the entire directory");
            // }

            if (String.IsNullOrEmpty(ldap_filter)) {
                throw new ArgumentException("LDAP query must not be empty");
            }

            this.Open();
            if (!String.IsNullOrEmpty(this.ObjectClass)) {
                ldap_filter = $"(&(objectclass={this.ObjectClass}){ldap_filter})";
            }

            IList<T> users = new List<T>();

            var properties = type.GetProperties();

            var search = _ldapConnection.Search(
                this._searchBase, LdapConnection.SCOPE_SUB, ldap_filter,
                null, false, null, null);
            LdapMessage message;

            while ((message = search.getResponse()) != null) {

                if (message.Type == LdapMessage.SEARCH_RESPONSE) {
                    var searchResultMessage = message as LdapSearchResult;
                    var entry = searchResultMessage.Entry;
                    var account = Activator.CreateInstance(type) as T;
                    account.LdapEntry = entry;
                    account.DistinguishName = entry.DN;

                    foreach (var property in properties) {
                        if(property.SetMethod != null) {
                            var attr = (DirectoryPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(DirectoryPropertyAttribute), false);

                            if (attr != null) {
                                if (property.PropertyType.IsArray) {
                                    var values = this.GetValues(entry, attr.SchemaAttributeName);
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
                                    var value = this.GetValue(entry, attr.SchemaAttributeName);
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

                } else {
                    var response = message as LdapResponse;
                    if (!String.IsNullOrEmpty(response.ErrorMessage)) {
                        throw new DirectoryServiceException(response.ErrorMessage);
                    }
                }
            }
            return users;
        }


        public virtual void Close() {
            if(this._ldapConnection != null) {
                this._ldapConnection.Disconnect();
            }
        }

        public virtual void Open() {
            if(_ldapConnection == null || _ldapConnection.Connected == false) {
                _ldapConnection = new LdapConnection() { SecureSocketLayer = this._ldapSettings.UseSSL };
                //Connect function will create a socket connection to the server - Port 389 for insecure and 3269 for secure    
                _ldapConnection.Connect(this._ldapSettings.ServerName, this._ldapSettings.ServerPort);
                //Bind function with null user dn and password value will perform anonymous bind to LDAP server
                if (_ldapConnection.AuthenticationMethod == "simple") {
                    //if (!String.IsNullOrEmpty(_ldapSettings.Bind.BindDn)) {
                        _ldapConnection.Bind(this._ldapSettings.Bind.BindDn, this._ldapSettings.Bind.BindCredential);
                    //}
                }
                // _ldapConnection.Bind(null, null);
            }
        }
        public virtual void Dispose() {
            this.Close();
            if(this._ldapConnection != null) {
                this._ldapConnection.Dispose();
            }
        }

        protected string[] GetValues(LdapEntry ldapEntry, string name) {
            var attributeSet = ldapEntry.getAttributeSet();
            var attribute = attributeSet.getAttribute(name);
            String[] values = new String[0];
            if (attribute != null) {
                values = attribute.StringValueArray;
            }
            return values;
        }

        protected string GetValue(LdapEntry ldapEntry, string name, int pos = -1) {
            var attributeSet = ldapEntry.getAttributeSet();
            var attribute = attributeSet.getAttribute(name);
            String[] values = new String[0];
            if (attribute != null) {
                values = attribute.StringValueArray;
            }
            string value = String.Empty;
            if (pos < 0) {
                pos = values.Length + pos;
            }
            pos = Math.Max(0, pos);
            if (values.Length > pos) {
                value = values[pos];
            }
            return value;
        }

    }

}
