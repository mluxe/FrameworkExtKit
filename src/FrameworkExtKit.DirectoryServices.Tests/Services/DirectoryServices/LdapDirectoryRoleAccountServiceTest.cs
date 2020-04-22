using FrameworkExtKit.Services.DirectoryServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {
    [Ignore("please update the unit test to fit your ldap structure")]
    public class LdapDirectoryRoleAccountServiceTest {


        IDirectoryRoleAccountService<DirectoryRoleAccount> service;

        public LdapDirectoryRoleAccountServiceTest() {
            service = new LdapDirectoryRoleAccountService<DirectoryRoleAccount>();
        }

        [SetUp]
        public void SetUp() {
            
        }


        [Test]
        public void test_find_by_alias() {
            var role = service.FindByAlias("it-role");

            validate_role_entity(role);
        }

        [Test]
        public void test_find_by_unique_id() {
            var role = service.FindByUniqueId("it-role");

            validate_role_entity(role);
        }

        [Test]
        public void test_find_by_owner() {
            var roles = service.FindByOwner("cn=Yufei Liu  442179,ou=employee,o=company,C=AN");

            Assert.AreEqual(17, roles.Count());
        }

        [Test]
        public void test_find_by_manager() {
            var roles = service.FindByManager("cn=Yufei Liu  449179,ou=employee,o=company,C=AN");
            Assert.AreEqual(17, roles.Count());
        }

        [Test]
        public void test_find_by_proxy() {
            var roles = service.FindByProxy("alias=lyufei");
            Assert.AreEqual(5, roles.Count());
        }

        private void validate_role_entity(DirectoryRoleAccount role) {
            Assert.AreEqual("GB", role.Country);
            Assert.AreEqual("IT", role.Organization);
            Assert.AreEqual("it-role", role.UniqueId);
            Assert.AreEqual(363107, role.Id);
            Assert.AreEqual("Horsham", role.City);
            Assert.AreEqual("role", role.EmployeeType);
            Assert.AreEqual("cn=IT-Role  363107,ou=role,o=company,c=an", role.DistinguishName);
            Assert.AreEqual("cn=Yufei Liu  898094,ou=employee,o=company,c=an", role.Manager);
            Assert.AreEqual("itrole@exchange.company.com", role.Mail);
        }



    }
}
