using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;


#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif

#if NETCORE
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {

    public partial class DirectoryEntity {

#if NET45
        public virtual SearchResult SearchResult { get; set; }
#endif

#if NETCORE
        public virtual LdapEntry LdapEntry { get; set; }
#endif
        [DirectoryProperty("id")]
        public virtual long Id { get; set; }
        [DirectoryProperty("objectClass")]
        public virtual string ObjectClass { get; set; }
        [DirectoryProperty("objectClass")]
        public virtual string[] ObjectClasses { get; set; }
        public virtual string DistinguishName { get; set; }
        [DirectoryProperty("ou")]

        public virtual string OrganizationUnit { get; set; }
        [DirectoryProperty("o")]
        public virtual string Organization { get; set; }
        [DirectoryProperty("c")]
        public virtual string Country { get; set; }
        [DirectoryProperty("cn")]
        public virtual string CommonName { get; set; }
        [DirectoryProperty("l")]
        public virtual string City { get; set; }
        [DirectoryProperty("uid")]
        public virtual string UniqueId { get; set; }
        public override string ToString() {
            PropertyInfo[] properties = this.GetType().GetProperties();
            StringBuilder output = new StringBuilder();
            foreach (var info in properties) {
                output.AppendLine(String.Format("{0}: {1}", info.Name, info.GetValue(this)));
            }

            return output.ToString();
        }

        public virtual string GetValue(string name) {
            return this.GetValues(name).FirstOrDefault();
        }

#if NET45
        public virtual string[] GetValues(string name) {
            var properties = SearchResult.Properties[name];
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
#endif
#if NETCORE
        protected string[] GetValues(string name) {
            var attributeSet = LdapEntry.getAttributeSet();
            var attribute = attributeSet.getAttribute(name);
            String[] values = new String[0];
            if (attribute != null) {
                values = attribute.StringValueArray;
            }
            return values;
        }
#endif
    }
}
