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
    public partial class DirectoryAccount {

        #region obsolete methods

        [Obsolete("replaced with DirectManager property")]
        public virtual string DirectManagerDN {
            get {
                return this.DirectManager;
            }
        }

        [Obsolete("replaced with Managers property")]
        public virtual string[] ManagerDNs {
            get {
                return this.Managers;
            }
        }

        #endregion

        [DirectoryProperty("EmployeeNumber")]
        public virtual string GIN { get; set; }
        [DirectoryProperty("id")]
        public virtual long[] ManagerDirectoryIDs {
            get {
                string[] managerDNs = this.Managers;
                long[] managerIDs = new long[managerDNs.Length];

                for (int i = 0; i < managerDNs.Length; i++) {
                    string dn = managerDNs[i];

                    dn = dn.Substring(dn.IndexOf('=') + 1);
                    dn = dn.Substring(0, dn.IndexOf(','));
                    dn = dn.Substring(dn.LastIndexOf(' ') + 1);
                    managerIDs[i] = Convert.ToInt32(dn);
                }
                return managerIDs;
            }
        }


        [DirectoryProperty("id")]
        public virtual long DirectManagerID {
            get {
                string dn = this.DirectManager;
                long id = 0;
                if (!String.IsNullOrEmpty(dn)) {
                    dn = dn.Substring(dn.IndexOf('=') + 1);
                    dn = dn.Substring(0, dn.IndexOf(','));
                    dn = dn.Substring(dn.LastIndexOf(' ') + 1);
                    id = Convert.ToInt64(dn);
                }

                return id;
            }
        }

        [DirectoryProperty("subscriptions")]
        public virtual string[] Subscriptions { get; set; }
        [DirectoryProperty("AccessRights")]
        public virtual string[] AccessRights { get; set; }
        [DirectoryProperty("JobCategory")]
        public virtual string JobCategory { get; set; }
        [DirectoryProperty("locationcode")]
        public virtual string LocationCode { get; set; }
        [DirectoryProperty("ActiveDirectoryDN")]
        public virtual string ActiveDirectoryDN { get; set; }
        [DirectoryProperty("BusinessCategory")]
        public virtual string BusinessCategory { get; set; }
        [DirectoryProperty("BusinessCategory")]
        public virtual string BusinessCategoryCode {
            get {
                if (this.BusinessCategory.Length > 4) {
                    return this.BusinessCategory.Substring(0, 4);
                }
                return String.Empty;
            }
        }
        [DirectoryProperty("employeetype")]
        public virtual string EmployeeType { get; set; }
        [DirectoryProperty("JobGroup")]
        public virtual string JobGroup { get; set; }
        [DirectoryProperty("JobGroup")]
        public virtual string JobGroupId {
            get {
                string longJobGroup = this.JobGroup;
                string code = String.Empty;
                if (longJobGroup.Length > 0) {
                    code = longJobGroup.Substring(0, longJobGroup.IndexOf('-'));
                }
                return code;
            }
        }
        public virtual string JobGroupName {
            get {
                string longJobGroup = this.JobGroup;
                string name = String.Empty;
                if (longJobGroup.Length > 0) {
                    int index = longJobGroup.IndexOf('-') + 1;
                    name = longJobGroup.Substring(index, longJobGroup.Length - index);
                }
                return name;
            }
        }
        [DirectoryProperty("JobCode")]
        public virtual string JobCode { get; set; }
        [DirectoryProperty("JobCode")]
        public virtual string JobCodeID {
            get {
                string longJobCode = this.JobCode;
                string code = String.Empty;
                if (longJobCode.Length > 0) {
                    code = longJobCode.Substring(0, longJobCode.IndexOf('-'));
                }
                return code;
            }
        }
        [DirectoryProperty("JobCode")]
        public virtual string JobCodeName {
            get {
                string longJobCode = this.JobCode;
                string name = String.Empty;
                if (longJobCode.Length > 0) {
                    int index = longJobCode.IndexOf('-') + 1;
                    name = longJobCode.Substring(index, longJobCode.Length - index );
                }
                return name;
            }
        }

        [DirectoryProperty("legalentitybillable")]
        public virtual string LegalEntity { get; set; }


        [DirectoryProperty("slbitbuilding")]
        public virtual string ITBuilding { get; set; }
        [DirectoryProperty("locationcostcode")]
        public virtual string AccountingCode { get; set; }
        [DirectoryProperty("locationcostcodehris")]
        public virtual string AccountingUnit { get; set; }

        public virtual string Geosite {
            get {
                if (!String.IsNullOrEmpty(this.ITBuilding) && this.ITBuilding.Length > 6) {
                    return this.ITBuilding.Substring(this.ITBuilding.Length - 6, 6);
                } else {
                    return String.Empty;
                }

            }
            set {
                throw new NotImplementedException("SetGeosite is not implemented for property Geosite");
            }
        }

        /**
         * the following properties are only available in the database table
         * it requires to process data from other data sources
         */
        public virtual string Geomarket { get; set; }
        public virtual string Area { get; set; }
        public virtual string SegmentCode { get; set; }
    }
}