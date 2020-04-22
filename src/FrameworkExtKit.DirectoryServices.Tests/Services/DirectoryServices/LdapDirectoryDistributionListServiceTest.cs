using FrameworkExtKit.Services.DirectoryServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {
    [Ignore("please update the unit test to fit your ldap structure")]
    public class LdapDirectoryDistributionListServiceTest {


        IDirectoryDistributionListService<DirectoryDistributionList> service;

        [SetUp]
        public void SetUp() {
            service = new LdapDirectoryDistributionListService<DirectoryDistributionList>();
        }


        [Test]
        public void test_find_by_alias() {
            var list = service.FindByAlias("it-ops");

            validate_list_entity(list);
        }

        [Test]
        public void test_find_by_unique_id() {
            var list = service.FindByUniqueId("itops-1291213");

            validate_list_entity(list);
        }

        [Test]
        public void test_find_by_owner() {
            var lists = service.FindByOwner("cn=Youlin Li  123123123,ou=employee,o=company,C=AN");

            Assert.AreEqual(8, lists.Count());
        }

        [Test]
        public void test_find_by_manager() {
            var lists = service.FindByManager("cn=Youlin Li  123123123,ou=employee,o=company,C=AN");
            Assert.AreEqual(8, lists.Count());
        }

        [Test]
        public void test_find_by_proxy() {
            var lists = service.FindByProxy("alias=lyufei");
            Assert.AreEqual(1, lists.Count());
        }

        private void validate_list_entity(DirectoryDistributionList list) {
            Assert.AreEqual("GB", list.Country);
            Assert.AreEqual("IT", list.Organization);
            Assert.AreEqual("it-ops", list.UniqueId);
            Assert.AreEqual(535768, list.Id);
            Assert.AreEqual("Horsham", list.City);
            Assert.AreEqual(8, list.Subscriptions.Length);
            Assert.AreEqual("list", list.EmployeeType);
            Assert.AreEqual("cn=ITOps  123123123,ou=list,o=company,c=an", list.DistinguishName);
            Assert.AreEqual("cn=Youlin Li  12312312,ou=employee,o=company,C=AN", list.ManagerDn);
            Assert.AreEqual("ldap@company.com", list.Mail);
        }



    }
}
