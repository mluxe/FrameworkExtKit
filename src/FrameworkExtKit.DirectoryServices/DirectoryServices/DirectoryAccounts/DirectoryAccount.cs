using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {

    /**
     * This is the base class of the DirectoryAccount
     * 
     *          
     * By: Yufei Liu <feilfly@gmail.com>
     * Date: 12th Nov, 2014 @ UK
     */
    public partial class DirectoryAccount : DirectoryEntity {

        [DirectoryProperty("mail")]
        public virtual string Mail { get; set; }
        [DirectoryProperty("department")]
        public virtual string Department { get; set; }
        [DirectoryProperty("telephonenumber")]
        public virtual string[] TelephoneNumbers { get; set; }
        [DirectoryProperty("mobile")]
        public virtual string[] MobileNumbers { get; set; }
        [DirectoryProperty("DisplayName")]
        public virtual string DisplayName { get; set; }
        [DirectoryProperty("sn")]
        public virtual string SurName { get; set; }
        [DirectoryProperty("GivenName")]
        public virtual string GivenName { get; set; }
        [DirectoryProperty("jobtitle")]
        public virtual string JobTitle { get; set; }
        [DirectoryProperty("alias")]
        public virtual string Alias { get; set; }
        [DirectoryProperty("street")]
        public virtual string Street { get; set; }
        [DirectoryProperty("postalcode")]
        public virtual string PostalCode { get; set; }
        [DirectoryProperty("manager")]
        public virtual string[] Managers { get; set; }
        [DirectoryProperty("manager")]
        public virtual string DirectManager {
            get {
                string[] managers = this.Managers;
                if (managers.Length > 0) {
                    return managers[0];
                }
                return String.Empty;
            }
        }

        [DirectoryProperty("manager")]
        public virtual string[] FunctionManagers {
            get {
                string[] managers = this.Managers;
                
                if (managers.Length > 1) {
                    string[] func_managers = new string[managers.Length - 1];
                    for(var i=1; i<managers.Length; i++) {
                        func_managers[i - 1] = managers[i];
                    }
                    return func_managers;
                }
                return new string[0];
            }
        }

        public virtual string OfficeAddress {
            get {
                return this.Street + ", " + this.City + ", " + this.Country + ", " + this.PostalCode;
            }
        }
    }
}