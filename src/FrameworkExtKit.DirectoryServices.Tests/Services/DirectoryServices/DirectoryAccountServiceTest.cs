using FrameworkExtKit.Services.DirectoryServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkExtKit.Services.Tests.DirectoryServices {
    public abstract partial class DirectoryAccountServiceTest {

        protected IDirectoryAccountService service;
        protected string userAlias = "LYufei";

        public DirectoryAccountServiceTest() {
        }

        [SetUp]
        public virtual void SetUp() {
            var msg = this.GetType().ToString();
            Assert.IsNotNull(service, "service must not be null in " + msg);
        }

        [TearDown]
        public virtual void TearDown() {
        }

        [Test]
        public void test_find_by_alias_should_success() {
            var account = service.FindByAlias("LYufei");
            this.test_ldap_record_yufei(account);
        }

        [Test]
        public void test_find_by_gin_should_success() {
            var account = service.FindByGIN("56787840");
            this.test_ldap_record_yufei(account);
        }

        [Test]
        public void test_find_by_id_should_success() {
            var account = service.FindById(721668);
            this.test_ldap_record_yufei(account);
        }

        [Test]
        public virtual void test_find_by_identity_should_success() {
            var account = service.FindByIdentifier("LYufei");
            this.test_ldap_record_yufei(account);

            //account = service.FindByIdentifier("04878765");
            //this.test_ldap_record_yufei(account);

            //account = service.FindByIdentifier("cn=Liu Yufei  100001,ou=contractor,o=company,C=AN");
            //this.test_ldap_record_yufei(account);

            account = service.FindByIdentifier("721668");
            this.test_ldap_record_yufei(account);

            account = service.FindByIdentifier("liu-20140317");
            this.test_ldap_record_yufei(account);
        }

        /*
         * there are some performance issues when we query active directory
         * comment out the code for the time being
        [Test]
        public void test_find_by_name_should_success() {
            var accounts = service.FindByName("Yufei Liu");
            Assert.AreEqual(1, accounts.Count);
            this.test_ldap_record_yufei(accounts[0]);
        }
        */
        [Test]
        public virtual void test_find_direct_manager() {
            DirectoryAccount user = service.FindByAlias(userAlias);

            DirectoryAccount manager = service.FindDirectManager(user);

            Assert.IsNotNull(manager);
            Assert.AreEqual(user.DirectManager.ToLower(), manager.DistinguishName.ToLower());
        }

        [Test]
        public void test_find_function_managers() {
            DirectoryAccount user = service.FindByAlias(userAlias);

            IEnumerable<DirectoryAccount> accounts = service.FindFunctionManagers(user);

            Assert.IsNotNull(accounts);
            Assert.AreEqual(0, accounts.Count());
        }

        [Test]
        public virtual void test_find_team_members() {
            DirectoryAccount user = service.FindByAlias("KLi5");

            IEnumerable<DirectoryAccount> accounts = service.FindTeamMembers(user);

            Assert.IsNotNull(accounts);
            Assert.IsTrue(accounts.Count() > 0);
            // we use >= 8 assertion is because 
            // AD is not fully synced to LDAP yet
            // so in AD Katherine has 9 reporters
            // where in LDAP Katherine only has 8 reporters
            Assert.AreEqual(2, accounts.Count());
        }

        [Test]
        public void test_reopen_connection() {
            service.Close();
            service.Open();

            service.Close();
            service.Open();

            var user = service.FindByAlias(userAlias);
            Assert.IsNotNull(user);
            Assert.AreEqual("56787840", user.GIN);
        }

        [Test]
        public void test_search_user_cannot_get_distribution_result() {
            // commented out because it takes too long to perform this search
            //var result = service.SearchUser("ITReporter");

            //Assert.AreEqual(0, result.Count);
        }

        /*
        [Test]
        public virtual void test_search_distribution_list() {
            var result = service.SearchDistributionList("ITReporter");

            Assert.AreEqual(1, result.Count);
        }
         */

        protected virtual void test_ldap_record_yufei(DirectoryAccount user) {
            var msg = this.GetType().ToString() + " unexpected value";
            throw new NotImplementedException("test_ldap_record_yufei must be implemented on sub classes, " + msg);
        }
    }
}
