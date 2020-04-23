/*
 Navicat Premium Data Transfer

 Source Server         : Windows 10
 Source Server Type    : SQL Server
 Source Server Version : 11003513
 Source Host           : 10.211.55.3
 Source Database       : DBDirectoryService
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11003513
 File Encoding         : utf-8

 Date: 02/26/2016 12:42:00 PM
*/

-- ----------------------------
--  Table structure for LDAPUsers
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID('[dbo].[LDAPUsers]') AND type IN ('U'))
	DROP TABLE [dbo].[LDAPUsers]
GO
CREATE TABLE [dbo].[LDAPUsers] (
	[Id] bigint IDENTITY(1,1) NOT NULL,
	[UserId] varchar(126) NOT NULL,
	[GIN] varchar(10) NOT NULL,
	[Mail] varchar(255) NOT NULL,
	[OrganizationUnit] varchar(126) NULL,
	[Department] varchar(126) NULL,
	[TelephoneNumber] varchar(126) NULL,
	[AccessRights] varchar(max) NULL,
	[EDMWorkStations] varchar(max) NULL,
	[DisplayName] varchar(126) NULL,
	[SurName] varchar(126) NOT NULL,
	[GivenName] varchar(126) NOT NULL,
	[JobTitle] varchar(126) NULL,
	[JobCategory] varchar(126) NULL,
	[LocationCode] varchar(126) NULL,
	[ActiveDirectoryDN] varchar(126) NULL,
	[BusinessCategory] varchar(126) NULL,
	[CommonName] varchar(256) NOT NULL,
	[EmployeeType] varchar(64) NULL,
	[JobGroup] varchar(126) NULL,
	[JobCode] varchar(126) NULL,
	[OrganizationName] varchar(126) NULL,
	[LegalEntity] varchar(126) NULL,
	[Alias] varchar(25) NOT NULL,
	[Street] varchar(max) NULL,
	[City] varchar(126) NULL,
	[Country] varchar(126) NULL,
	[PostalCode] varchar(126) NULL,
	[DistinguishName] varchar(256) NOT NULL,
	[ManagerDNs] varchar(max) NULL,
	[Subscriptions] varchar(max) NULL,
	[ObjectClass] varchar(25) NOT NULL DEFAULT '',
	[Geosite] varchar(8) NULL,
	[Geomarket] varchar(25) NULL,
	[Area] varchar(10) NULL,
	[ITBuilding] varchar(125) NULL,
	[SegmentCode] varchar(10) NULL,
	[AccountingCode] varchar(20) NULL,
	[AccountingUnit] varchar(20) NULL,
	[MobileNumber] varchar(126) NULL
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

-- ----------------------------
--  Records of LDAPUsers
-- ----------------------------
BEGIN TRANSACTION
GO
SET IDENTITY_INSERT [dbo].[LDAPUsers] ON
GO
INSERT INTO [dbo].[LDAPUsers] ([Id], [UserId], [GIN], [Mail], [OrganizationUnit], [Department], [TelephoneNumber], [AccessRights], [EDMWorkStations], [DisplayName], [SurName], [GivenName], [JobTitle], [JobCategory], [LocationCode], [ActiveDirectoryDN], [BusinessCategory], [CommonName], [EmployeeType], [JobGroup], [JobCode], [OrganizationName], [LegalEntity], [Alias], [Street], [City], [Country], [PostalCode], [DistinguishName], [ManagerDNs], [Subscriptions], [ObjectClass], [Geosite], [Geomarket], [Area], [ITBuilding], [SegmentCode], [AccountingCode], [AccountingUnit], [MobileNumber]) 
VALUES ('29381662', 'Youlin-20080902', '238899', 'KLi5@company.com', '00067886', 'IT', '+86 17177 64 28', 'MobileAccess
RemoteAccess', '', 'Youlin Li', 'Li', 'Youlin', 'IT Solutions Manager', ' Data Processing', '4482733', 'CN=Youlin Li  29381662,ou=Users,OU=Houston,OU=US,OU=nam,DC=DIR,DC=company,DC=com', null, 'Youlin Li 2', 'employee', '00001540-JG', '82154002-JC', 'IT', 'IT Operations', 'KLi5', '1430 ,  Virgiia,  GBP', 'Houston', 'US', '77077', 'cn=Youlin Li  29381662,ou=employee,o=company,C=AN', 'cn=Yufei Liu  156759,ou=employee,o=company,c=AN', null, 'person', 'US0001', 'USA-Local', 'USA', 'Virginia', 'IT Ops', '4482733', '0004482751', null);
INSERT INTO [dbo].[LDAPUsers] ([Id], [UserId], [GIN], [Mail], [OrganizationUnit], [Department], [TelephoneNumber], [AccessRights], [EDMWorkStations], [DisplayName], [SurName], [GivenName], [JobTitle], [JobCategory], [LocationCode], [ActiveDirectoryDN], [BusinessCategory], [CommonName], [EmployeeType], [JobGroup], [JobCode], [OrganizationName], [LegalEntity], [Alias], [Street], [City], [Country], [PostalCode], [DistinguishName], [ManagerDNs], [Subscriptions], [ObjectClass], [Geosite], [Geomarket], [Area], [ITBuilding], [SegmentCode], [AccountingCode], [AccountingUnit], [MobileNumber]) 
VALUES ('721668', 'liu-20140317', '56787840', 'LYufei@company.com', 'Shared Services', 'IT', '+44 1293 557048', 'BasicAccess
FullAccess
RemoteAccess', '', 'Yufei Liu', 'Liu', 'Yufei', 'Contractor', 'Data Processing', 'GB0008', 'CN=Liu Yufei  100001,ou=Users,OU=Horsham,OU=GB,DC=DIR,DC=company,DC=com', 'IT Ops', 'Yufei Liu', 'contractor', '00001540-JG', '82154011-JC', 'IT', 'IT Operations', 'LYufei', 'West Sussex', 'Gatwick', 'GB', 'RH6 6HR', 'cn=Liu Yufei  100001,ou=contractor,o=company,C=AN', 'cn=Youlin Li  29381662,ou=employee,o=company,C=AN', 'Java-Pre-Pilot', 'person', 'GB0001', 'UKG', 'EAF', 'Gatwick', 'IT Ops', null, '0004482751', null);
GO
SET IDENTITY_INSERT [dbo].[LDAPUsers] OFF
GO
COMMIT
GO


-- ----------------------------
--  Primary key structure for table LDAPUsers
-- ----------------------------
ALTER TABLE [dbo].[LDAPUsers] ADD
	CONSTRAINT [PK__LDAPUser__3214EC077767379D] PRIMARY KEY CLUSTERED ([Id]) 
	WITH (PAD_INDEX = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON)
	ON [default]
GO

-- ----------------------------
--  Indexes structure for table LDAPUsers
-- ----------------------------
CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_Alias]
ON [dbo].[LDAPUsers] ([Alias] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_CommonName]
ON [dbo].[LDAPUsers] ([CommonName] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_DisplayName]
ON [dbo].[LDAPUsers] ([DisplayName] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_DN]
ON [dbo].[LDAPUsers] ([DistinguishName] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_LDAPUsers_GIN]
ON [dbo].[LDAPUsers] ([GIN] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_GivenName]
ON [dbo].[LDAPUsers] ([GivenName] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_LocationCode]
ON [dbo].[LDAPUsers] ([LocationCode] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_Mail]
ON [dbo].[LDAPUsers] ([Mail] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_LDAPUsers_SirName]
ON [dbo].[LDAPUsers] ([SurName] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_LDAPUsers_UserId]
ON [dbo].[LDAPUsers] ([UserId] ASC)
WITH (PAD_INDEX = OFF,
	IGNORE_DUP_KEY = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	SORT_IN_TEMPDB = OFF,
	ONLINE = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO

-- ----------------------------
--  Options for table LDAPUsers
-- ----------------------------
ALTER TABLE [dbo].[LDAPUsers] SET (LOCK_ESCALATION = TABLE)
GO
DBCC CHECKIDENT ('[dbo].[LDAPUsers]', RESEED, 722667)
GO

