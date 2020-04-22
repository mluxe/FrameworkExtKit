using FrameworkExtKit.Services.DirectoryServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {
    [Ignore("please update the unit test to fit your ldap structure")]
    public class LdapDirectoryGroupServiceTest {


        IDirectoryGroupService<DirectoryGroup> service;

        public LdapDirectoryGroupServiceTest() {
            service = new LdapDirectoryGroupService<DirectoryGroup>();
        }

        [SetUp]
        public void SetUp() {
            
        }


        [Test]
        public void test_find_by_alias() {
            var group = service.FindByAlias("ITGroup");

            validate_group_entity(group);
        }

        [Test]
        public void test_find_by_owner() {
            var groups = service.FindByOwner("cn=Yufei Liu  425179,ou=employee,o=company,C=AN");

            Assert.AreEqual(11, groups.Count());
        }

        [Test]
        public void test_find_by_proxy() {
            var groups = service.FindByProxy("name=LYufei");
            Assert.AreEqual(7, groups.Count());
        }

        private void validate_group_entity(DirectoryGroup group) {
            Assert.AreEqual("IT Group", group.CommonName);
            Assert.AreEqual("", group.Organization);
            Assert.AreEqual("", group.UniqueId);
            Assert.AreEqual(12211396188, group.Id);
            Assert.AreEqual("", group.City);
            Assert.AreEqual(3, group.Members.Length);
            Assert.AreEqual("group", group.EmployeeType);
            Assert.AreEqual("cn=ITGropu,ou=MG,ou=group,o=company,c=an", group.DistinguishName);
            Assert.AreEqual("cn=Yufei Liu  441179,ou=employee,o=company,C=AN", group.Owner);
        }



    }
}
