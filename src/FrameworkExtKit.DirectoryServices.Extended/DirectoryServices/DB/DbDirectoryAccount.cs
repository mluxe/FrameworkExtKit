using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif

#if NETCORE
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
#endif

namespace FrameworkExtKit.Services.DirectoryServices.DB {
    /**
     * 
     * The DBDirectoryAccount class is a sub class of the DirectoryAccount
     * 
     * By: Yufei Liu <yliu@leyun.co.uk>
     * Date: 8th May, 2015 @ Gatwick, UK
     * 
     * SQL to create the LDAPUsers Database
     CREATE TABLE [dbo].[LDAPUsers] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [UserId] varchar(126) COLLATE Latin1_General_CI_AS NOT NULL,
        [GIN] varchar(10) COLLATE Latin1_General_CI_AS NOT NULL,
        [Mail] varchar(255) COLLATE Latin1_General_CI_AS NOT NULL,
        [OrganizationUnit] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [Department] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [TelephoneNumber] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [AccessRights] varchar(max) COLLATE Latin1_General_CI_AS NULL,
        [EDMWorkStations] varchar(max) COLLATE Latin1_General_CI_AS NULL,
        [Subscriptions] varchar(max) COLLATE Latin1_General_CI_AS NULL,
        [DisplayName] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [SirName] varchar(126) COLLATE Latin1_General_CI_AS NOT NULL,
        [GivenName] varchar(126) COLLATE Latin1_General_CI_AS NOT NULL,
        [JobTitle] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [JobCategory] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [LocationCode] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [ActiveDirectoryDN] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [BusinessCategory] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [CommonName] varchar(256) COLLATE Latin1_General_CI_AS NOT NULL,
        [EmployeeType] varchar(64) COLLATE Latin1_General_CI_AS NULL,
        [JobGroup] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [JobCode] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [OrganizationName] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [Alias] varchar(25) COLLATE Latin1_General_CI_AS NOT NULL,
        [Street] varchar(max) COLLATE Latin1_General_CI_AS NULL,
        [City] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [Country] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [PostalCode] varchar(126) COLLATE Latin1_General_CI_AS NULL,
        [DistinguishName] varchar(256) COLLATE Latin1_General_CI_AS NOT NULL,
        [ManagerDNs] varchar(max) COLLATE Latin1_General_CI_AS NULL,
        PRIMARY KEY CLUSTERED ([Id]) 
        WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    )
    ON [PRIMARY]

    -- ----------------------------
    --  Indexes structure for table LDAPUsers
    -- ----------------------------
    CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_Alias]
    ON [dbo].[LDAPUsers] ([Alias] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF,
	    SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    
    CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_CommonName]
    ON [dbo].[LDAPUsers] ([CommonName] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF,
	    SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    
    CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_DisplayName]
    ON [dbo].[LDAPUsers] ([DisplayName] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF,
            SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_DN]
    ON [dbo].[LDAPUsers] ([DistinguishName] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF,
	    SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    CREATE UNIQUE NONCLUSTERED INDEX [IDX_LDAPUsers_GIN]
    ON [dbo].[LDAPUsers] ([GIN] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_GivenName]
    ON [dbo].[LDAPUsers] ([GivenName] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF,
	    SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_LocationCode]
    ON [dbo].[LDAPUsers] ([LocationCode] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF,
	    ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_Mail]
    ON [dbo].[LDAPUsers] ([Mail] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF,
	    SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_SirName]
    ON [dbo].[LDAPUsers] ([SirName] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF,
	    ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    CREATE UNIQUE NONCLUSTERED INDEX [IDX_LDAPUsers_UserId]
    ON [dbo].[LDAPUsers] ([UserId] ASC)
    WITH (PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF,
	    ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    ON [PRIMARY]
    GO
    
     */
    [Table("LDAPUsers")]
    public partial class DbDirectoryAccount : DirectoryAccount{

        public DbDirectoryAccount() {

        }

#if NET45
        [NotMapped]
        public override SearchResult SearchResult { get; set; }
#endif

#if NETCORE
        [NotMapped]
        public override LdapEntry LdapEntry { get; set; }
#endif

        [NotMapped]
        public override string[] ObjectClasses { get => base.ObjectClasses; set => base.ObjectClasses = value; }

        private string[] SplitStringElement(string text) {
            var elements = text.Split(new string[] { "\r\n", "\n", "\r", ";" }, StringSplitOptions.RemoveEmptyEntries);
            for(var i=0; i<elements.Length; i++){
                elements[i] = elements[i].Trim();
            }
            return elements;
        }

        [NotMapped]
        public override string Organization { 
            get { return this.OrganizationName; } 
            set { this.OrganizationName = value; }
        }

        [Column("OrganizationName")]
        public virtual string OrganizationName { get; set; }

        [Column("ManagerDNs")]
        [JsonIgnore]
        public virtual string ManagerDNString { get; set; }

        [NotMapped]
        public override string[] Managers {
            get {
                return this.SplitStringElement(ManagerDNString);
            }
        }
        [NotMapped]
        public override string DirectManager {
            get {
                return base.DirectManager;
            }
        }

        [Column("AccessRights")]
        [JsonIgnore]
        public virtual string AccessRightsString { get; set; }
        [NotMapped]
        public override string[] AccessRights {
            get {
                if (String.IsNullOrEmpty(AccessRightsString)) {
                    return new string[0];
                }
                return this.SplitStringElement(AccessRightsString);
            }
        }

        [Column("Subscriptions")]
        [JsonIgnore]
        public virtual string SubscriptionsString { get; set; }

        [NotMapped]
        public override string[] Subscriptions {
            get {
                if (String.IsNullOrEmpty(SubscriptionsString)) {
                    return new string[0];
                }

                return this.SplitStringElement(SubscriptionsString);
            }
        }

        [Column("TelephoneNumber")]
        [JsonIgnore]
        public virtual string TelephoneNumberString { get; set; }

        [NotMapped]
        public override string[] TelephoneNumbers {
            get {
                if (String.IsNullOrEmpty(TelephoneNumberString)) {
                    return new string[0];
                }

                return this.SplitStringElement(TelephoneNumberString);
            }
        }

        [Column("MobileNumber")]
        [JsonIgnore]
        public virtual string MobileNumberString { get; set; }

        [NotMapped]
        public override string[] MobileNumbers {
            get {
                if (String.IsNullOrEmpty(MobileNumberString)) {
                    return new string[0];
                }

                return this.SplitStringElement(MobileNumberString);
            }
        }

        [NotMapped]
        public override string OfficeAddress {
            get {
                return base.OfficeAddress;
            }
        }
        [NotMapped]
        public override string JobCodeID {
            get {
                return base.JobCodeID;
            }
        }
        [NotMapped]
        public override string JobCodeName {
            get {
                return base.JobCodeName;
            }
        }
        [NotMapped]
        public override string JobGroupId {
            get {
                return base.JobGroupId;
            }
        }
        [NotMapped]
        public override string JobGroupName {
            get {
                return base.JobGroupName;
            }
        }
        [NotMapped]
        public override long DirectManagerID {
            get {
                return base.DirectManagerID;
            }
        }
        [NotMapped]
        public override long[] ManagerDirectoryIDs {
            get {
                return base.ManagerDirectoryIDs;
            }
        }
        
        [Column("SegmentCode")]
        public override string SegmentCode { get; set; }
        [Column("UserId")]
        public override string UniqueId { get; set; }

        public override string Geosite { get; set; }
    }
}
