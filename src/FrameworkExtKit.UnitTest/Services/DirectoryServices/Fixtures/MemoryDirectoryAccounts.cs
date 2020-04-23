using FrameworkExtKit.Services.DirectoryServices;
using System;
using System.Collections.Generic;
using System.Reflection;



namespace FrameworkExtKit.UnitTest.Services.DirectoryServices.Fixtures {

    /**
     *
     * This static class hosts all the in memory LDAP accounts
     * that we need to use for unit testing
     * 
     * it should be consistant with the fixture data in CoreData API
     * 
     */
    public static partial class MemoryDirectoryAccounts {
        private static List<DirectoryAccount> memoryDirectoryAccounts = new List<DirectoryAccount> ();

        #region accounts in the memory
        /**
         * Please use the MemoryDirectoryAccountGenerator.php  in Utilities folder
         * to generate the DirectoryAccount code
         */
        // System Role
        #region sysadmin
        public static DirectoryAccount SysAdmin = new DirectoryAccount() {
            ObjectClass = "Person",
            Alias = "sysadmin",
            SurName = "Administrator",
            GivenName = "System",
            DisplayName = "System Administrator",
            EmployeeType = "employee",
            BusinessCategory = "IT Operations",
            AccessRights = new string[] { "RemoteAccess" },
            CommonName = "System Administrator  000001",
            ActiveDirectoryDN = "CN=System Administrator  00001,ou=Users,OU=Virgiia,OU=GB,DC=DIR,DC=sample,DC=com",
            City = "Paris",
            Country = "CN",
            Department = "IT",
            DistinguishName = "cn=System Administrator  00001,ou=employee,o=sample,c=an",
            GIN = "00000001",
            Id = 1,
            JobCategory = "Management",
            JobCode = "82154002",
            JobGroup = "00001540",
            JobTitle = "Global IT - Administrator",
            LocationCode = "2450771",
            Mail = "sysadmin@sample.com",
            Managers = new string[] { "cn=System Administrator  00001,ou=employee,o=sample,c=an" },
            Organization = "IT",
            OrganizationUnit = "Corporate",
            PostalCode = "83770",
            Street = "#1 Unit Testing, Sample",
            TelephoneNumbers = new string[0],
            MobileNumbers = new string[0],
            UniqueId = "sysadmin-19000101",
            ITBuilding = "GB0080",
            AccountingUnit = "2450771O62",
            AccountingCode = "25012090013"
        };
        #endregion

        #region Unregisterd Dummy User
        public static DirectoryAccount UnregisteredUser = new DirectoryAccount() {
            ObjectClass = "Person",
            Alias = "Unregistered-User",
            SurName = "Unregistered",
            GivenName = "User",
            DisplayName = "Unregistered User",
            EmployeeType = "employee",
            BusinessCategory = "IT Operations",
            AccessRights = new string[] { "RemoteAccess" },
            CommonName = "Unregistered User  000002",
            ActiveDirectoryDN = "CN=Unregistered User  000002,ou=Users,OU=Virgiia,OU=GB,DC=DIR,DC=sample,DC=com",
            City = "Paris",
            Country = "CN",
            Department = "IT",
            DistinguishName = "cn=Unregistered User  000002,ou=employee,o=sample,c=an",
            GIN = "00000002",
            Id = 1,
            JobCategory = "Management",
            JobCode = "82154002",
            JobGroup = "00001540",
            JobTitle = "Global IT - Administrator",
            LocationCode = "2450771",
            Mail = "unregistered-user@sample.com",
            Managers = new string[] { "cn=System Administrator  00001,ou=employee,o=sample,c=an" },
            Organization = "IT",
            OrganizationUnit = "Corporate",
            PostalCode = "77092",
            Street = "#1 Unit Testing, Sample",
            TelephoneNumbers = new string[0],
            MobileNumbers = new string[0],
            UniqueId = "unregistereduser-19000101",
            ITBuilding = "GB0080",
            AccountingUnit = "2450771O62",
            AccountingCode = "25012090013"
        };
        #endregion
        // Team members

        #region Alan 
        public static DirectoryAccount ALoon = new DirectoryAccount() {
            ObjectClass = "Person",
            Alias = "ARoony",
            SurName = "Roony",
            GivenName = "Alan",
            DisplayName = "Alan Roony",
            EmployeeType = "employee",
            BusinessCategory = "IT Infrastructure",
            AccessRights = new string[] { "MobileAccess" },
            CommonName = "Alan Roony  82871222",
            ActiveDirectoryDN = "CN=Alan Roony  82871222,ou=Users,OU=La Defense-CN0016,OU=CN,DC=DIR,DC=sample,DC=com",
            City = "La Defense",
            Country = "CN",
            Department = "IT",
            DistinguishName = "cn=Alan Roony  82871222,ou=employee,o=sample,c=an",
            GIN = "000000004",
            Id = 156759,
            JobCategory = "Management",
            JobCode = "82152001",
            JobGroup = "00001520",
            JobTitle = "Quality and Process Manager",
            LocationCode = "2450771",
            Mail = "ALoon@sample.com",
            Managers = new string[] { "cn=Sebastien Lehnherr  212444,ou=employee,o=sample,c=an" },
            Organization = "IT",
            OrganizationUnit = "Corporate",
            PostalCode = "92936",
            Street = "92936 La Defense Cedex - France",
            TelephoneNumbers = new string[] { "+86 1 71 77 64 28" },
            MobileNumbers = new string[] { "+86 6 22 78 39 27" },
            Subscriptions = new string[] { "SCCMPilot2014", "Software-Distribution-Office 2016" },
            UniqueId = "alan-19980615",
            ITBuilding = "GB0080",
            AccountingUnit = "2450771O62",
            AccountingCode = "25012090013"
        };
        #endregion
        #region KLi5
        public static DirectoryAccount KLi5 = new DirectoryAccount() {
            ObjectClass = "Person",
            Alias = "KLi5",
            SurName = "Li",
            GivenName = "Youlin",
            DisplayName = "Youlin Li",
            EmployeeType = "employee",
            BusinessCategory = "IT Operations",
            AccessRights = new string[] { "PKIUser", "MobileAccess", "RemoteAccess" },
            CommonName = "Youlin Li  29381662",
            ActiveDirectoryDN = "CN=Youlin Li  29381662,ou=Users,OU=Virgiia,OU=GB,DC=DIR,DC=sample,DC=com",
            City = "Horsham",
            Country = "GB",
            Department = "IT",
            DistinguishName = "cn=Youlin Li  29381662,ou=employee,o=sample,c=an",
            GIN = "00000005",
            Id = 29381662,
            JobCategory = "Data Processing",
            JobCode = "82154002",
            JobGroup = "00001540",
            JobTitle = "IT Solutions Manager",
            LocationCode = "4482733",
            Mail = "KLi5@sample.com",
            Managers = new string[] { "cn=Alan Roony  82871222,ou=employee,o=sample,c=an" },
            Organization = "IT",
            OrganizationUnit = "IT Operations",
            PostalCode = "77077",
            Street = "1430 ,  Virgiia,  GBP",
            TelephoneNumbers = new string[] { "+44 832 462 2411" },
            MobileNumbers = new string[] { "+44 281 943 9589" },
            Subscriptions = new string[] { "it-software-metier" },
            UniqueId = "katherine-20080902",
            ITBuilding = "GB0080",
            AccountingUnit = "0004482751",
            AccountingCode = "4482733"
        };
        #endregion
        #region LYufei
        public static DirectoryAccount LYufei = new DirectoryAccount() {
            ObjectClass = "Person",
            Alias = "LYufei",
            SurName = "Liu",
            GivenName = "Yufei",
            DisplayName = "Yufei Liu",
            EmployeeType = "contractor",
            BusinessCategory = "IT Operations",
            AccessRights = new string[] { "RemoteAccess" },
            CommonName = "Liu Yufei  721668",
            ActiveDirectoryDN = "CN=Liu Yufei  721668,ou=Users,OU=Gatwick-GB0080,OU=GB,DC=DIR,DC=sample,DC=com",
            City = "Gatwick",
            Country = "GB",
            Department = "IT",
            DistinguishName = "cn=Liu Yufei  721668,ou=contractor,o=sample,c=an",
            GIN = "56787840",
            Id = 721668,
            JobCategory = "Data Processing",
            JobCode = "82154011-JC",
            JobGroup = "00001530-JG",
            JobTitle = "Engineer",
            LocationCode = "GB0008",
            Mail = "LYufei@sample.com",
            Managers = new string[] { "cn=Youlin Li  29381662,ou=employee,o=sample,c=an" },
            Organization = "IT",
            OrganizationUnit = "IT Operations",
            PostalCode = "RH6 6NZ",
            Street = "Buckingham Gate,  West Sussex",
            TelephoneNumbers = new string[] { "+44 1293 557048" },
            MobileNumbers = new string[] { },
            Subscriptions = new string[] { "Java-Pre-Pilot", "DCO-Newsletter", "macuserbb" },
            UniqueId = "liu-20140317",
            ITBuilding = "GB0080",
            AccountingUnit = "0004482751",
            AccountingCode = ""
        };
        #endregion
        #region VGrandhi
        public static DirectoryAccount VGrandhi = new DirectoryAccount() {
            ObjectClass = "Person",
            Alias = "VGrandhi",
            SurName = "Grandhi",
            GivenName = "Visali",
            DisplayName = "Visali Grandhi",
            EmployeeType = "contractor",
            BusinessCategory = "IT Operations",
            AccessRights = new string[] { "RemoteAccess" },
            CommonName = "Visali Grandhi  83827122",
            ActiveDirectoryDN = "CN=Visali Grandhi  83827122,ou=Users,OU=Virgiia,OU=GB,DC=DIR,DC=sample,DC=com",
            City = "Horsham",
            Country = "GB",
            Department = "IT",
            DistinguishName = "cn=Visali Grandhi  83827122,ou=contractor,o=sample,c=an",
            GIN = "02188605",
            Id = 333669,
            JobCategory = "Data Processing",
            JobCode = "82140213",
            JobGroup = "00005000",
            JobTitle = "Developer",
            LocationCode = "",
            Mail = "vgrandhi@sample.com",
            Managers = new string[] { "cn=Youlin Li  29381662,ou=employee,o=sample,c=an" },
            Organization = "IT",
            OrganizationUnit = "IT Operations",
            PostalCode = "77077",
            Street = "1430 Enclave Parkway",
            TelephoneNumbers = new string[] { "+44 832 462 2542" },
            MobileNumbers = new string[] { "+44 888 499 50000" },
            Subscriptions = new string[] { "eJourney" },
            UniqueId = "visali-20041104",
            ITBuilding = "GB0080",
            AccountingUnit = "0000883454",
            AccountingCode = ""
        };
        #endregion

        #endregion

        public static List<DirectoryAccount> ToList() {
            if (memoryDirectoryAccounts.Count == 0) {
                Type type = typeof(MemoryDirectoryAccounts);
                Type directoryAccountType = typeof(DirectoryAccount);

                FieldInfo[] fieldInfos = type.GetFields();

                foreach (var fieldInfo in fieldInfos) {
                    Type fieldType = fieldInfo.FieldType;

                    if (fieldType.FullName == directoryAccountType.FullName) {
                        var fieldValue = fieldInfo.GetValue(fieldInfo.Name);
                        memoryDirectoryAccounts.Add((DirectoryAccount)fieldValue);
                    }
                }
            }

            return memoryDirectoryAccounts;
        }
    }
}
