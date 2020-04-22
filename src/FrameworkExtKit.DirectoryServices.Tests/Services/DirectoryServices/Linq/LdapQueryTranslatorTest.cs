using FrameworkExtKit.Services.DirectoryServices;
using FrameworkExtKit.Services.DirectoryServices.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FrameworkExtKit.Services.Tests.DirectoryServices.Linq {
    [TestFixture]
	[Author("Yufei Liu", "yliu@leyun.co.uk")]
    public class LdapQueryTranslatorTest {

        LdapQueryTranslator translator;

        [SetUp]
        public void SetUp() {
        }

        [Test]
        public void test_translate_or_else_expression() {
            translator = this.GetTranslator(a => a.Alias == "YLiu" || a.Alias == "ALoon" || a.Alias == "KLi5");
            Assert.AreEqual("(|(alias=YLiu)(alias=ALoon)(alias=KLi5))", translator.FilterString);
        }

        [Test]
        public void test_translate_and_also_expression() {
            translator = this.GetTranslator(a => a.Alias == "YLiu" && a.Alias == "ALoon" && a.Alias == "KLi5");
            Assert.AreEqual("(&(alias=YLiu)(alias=ALoon)(alias=KLi5))", translator.FilterString);
        }

        [Test]
        public void test_translate_mixed_and_also_or_else_expression() {
            translator = this.GetTranslator(a => (a.Alias == "YLiu" || a.Alias == "ALoon" || a.Alias =="KLi5") && a.Alias == "KLi5");
            Assert.AreEqual("(&(|(alias=YLiu)(alias=ALoon)(alias=KLi5))(alias=KLi5))", translator.FilterString);
        }

        [Test]
        public void test_translate_not_equal_expression() {
            translator = this.GetTranslator(a => a.Alias != "YLiu");
            Assert.AreEqual("(!(alias=YLiu))", translator.FilterString);
        }

        [Test]
        public void test_translate_equal_expression() {
            translator = this.GetTranslator(a => a.Alias == "YLiu");
            Assert.AreEqual("(alias=YLiu)", translator.FilterString);
        }

        [Test]
        public void test_translate_expression_with_local_variable_constant() {
            string variable = "ALoon";
            translator = this.GetTranslator(a => a.Alias == variable);
            Assert.AreEqual("(alias=ALoon)", translator.FilterString);
        }

        [Test]
        public void test_translate_greater_than_expression() {
            translator = this.GetTranslator(a => a.Id > 0);
            Assert.AreEqual("(id>0)", translator.FilterString);
        }

        [Test]
        public void test_translate_greater_than_or_equal_expression() {
            translator = this.GetTranslator(a => a.Id >= 0);
            Assert.AreEqual("(id>=0)", translator.FilterString);
        }

        [Test]
        public void test_translate_less_than_expression() {
            translator = this.GetTranslator(a => a.Id < 100);
            Assert.AreEqual("(id<100)", translator.FilterString);
        }

        [Test]
        public void test_translate_less_than_or_equal_expression() {
            translator = this.GetTranslator(a => a.Id <= 100);
            Assert.AreEqual("(id<=100)", translator.FilterString);
        }

        [Test]
        public void test_translate_string_contains_expression() {
            translator = this.GetTranslator(a => a.DisplayName.Contains("Yufei"));
            Assert.AreEqual("(DisplayName=*Yufei*)", translator.FilterString);
        }

        [Test]
        public void test_translate_string_contains_expression_withlocal_variable_constant() {
            string variable = "Yufei";
            translator = this.GetTranslator(a => a.DisplayName.Contains(variable));
            Assert.AreEqual("(DisplayName=*Yufei*)", translator.FilterString);
        }

        [Test]
        public void test_translate_string_contains_expression_with_local_variable_to_string_converted() {
            int variable = 12;
            
            translator = this.GetTranslator(a => a.DisplayName.Contains(variable.ToString()));
            Assert.AreEqual("(DisplayName=*12*)", translator.FilterString);

            List<int> variable1 = new List<int>{
                2, 3, 4
            };
            translator = this.GetTranslator(a => a.DisplayName.Contains(variable1.ToString()));
            Assert.AreEqual("(DisplayName=*System.Collections.Generic.List`1[System.Int32]*)", translator.FilterString);
            
            translator = this.GetTranslator(a => a.City == variable.ToString());
            Assert.AreEqual("(l=12)", translator.FilterString);

            translator = this.GetTranslator(a => a.City == variable1.ToString());
            Assert.AreEqual("(l=System.Collections.Generic.List`1[System.Int32])", translator.FilterString);
        }

        [Test]
        public void test_translate_starts_with_expression() {
            translator = this.GetTranslator(a => a.DisplayName.StartsWith("Yufei"));
            Assert.AreEqual("(DisplayName=Yufei*)", translator.FilterString);
        }

        [Test]
        public void test_translate_ends_with_expression() {
            translator = this.GetTranslator(a => a.DisplayName.EndsWith("Yufei"));
            Assert.AreEqual("(DisplayName=*Yufei)", translator.FilterString);
        }

        [Test]
        public void test_translate_property_access_with_expression() {
            DirectoryAccount account = new DirectoryAccount {
                Alias = "Dummy", GivenName = "GivN", SurName = "SirN", Id = 132
            };

            translator = this.GetTranslator(a => a.Alias != account.GivenName);
            Assert.AreEqual("(!(alias=GivN))", translator.FilterString);

            translator = this.GetTranslator(a => account.GivenName != a.Alias);
            Assert.AreEqual("(!(alias=GivN))", translator.FilterString);

            translator = this.GetTranslator(a => a.Alias == account.GivenName);
            Assert.AreEqual("(alias=GivN)", translator.FilterString);

            translator = this.GetTranslator(a => account.GivenName == a.Alias);
            Assert.AreEqual("(alias=GivN)", translator.FilterString);

            translator = this.GetTranslator(a => a.Id == account.Id);
            Assert.AreEqual("(id=132)", translator.FilterString);

            translator = this.GetTranslator(a => account.Id == a.Id);
            Assert.AreEqual("(id=132)", translator.FilterString);
        }

        [Test]
        public void test_translate_multi_level_property_access_with_expression() {
            DirectoryAccount account = new DirectoryAccount {
                Alias = "Dummy", GivenName = "GivN", SurName = "SirN", Id = 132
            };
            var level1 = new {
                level2 = new {
                    level3 = new {
                        acc = account
                    }
                }
            };

            translator = this.GetTranslator(a => a.Alias != level1.level2.level3.acc.GivenName);
            Assert.AreEqual("(!(alias=GivN))", translator.FilterString);

            translator = this.GetTranslator(a => level1.level2.level3.acc.GivenName != a.Alias);
            Assert.AreEqual("(!(alias=GivN))", translator.FilterString);

            translator = this.GetTranslator(a => a.Alias == level1.level2.level3.acc.GivenName);
            Assert.AreEqual("(alias=GivN)", translator.FilterString);

            translator = this.GetTranslator(a => level1.level2.level3.acc.GivenName == a.Alias);
            Assert.AreEqual("(alias=GivN)", translator.FilterString);

            translator = this.GetTranslator(a => a.Id == level1.level2.level3.acc.Id);
            Assert.AreEqual("(id=132)", translator.FilterString);

            translator = this.GetTranslator(a => level1.level2.level3.acc.Id == a.Id);
            Assert.AreEqual("(id=132)", translator.FilterString);
        }

        [Test]
        public void test_translate_array_contains_expression() {
            string[] aliases = new string[3]{
                "LYufei", "ALoon", "KLi5"
            };

            //translator = this.GetTranslator(a => new string[2]{"L","C"}.Contains(a.Alias));
            translator = this.GetTranslator(a => aliases.Contains(a.Alias));
            Assert.AreEqual("(|(alias=LYufei)(alias=ALoon)(alias=KLi5))", translator.FilterString);
        }

        [Test]
        public void test_translate_string_list_contains_expression() {
            IEnumerable<String> aliases = new List<string>{
                "LYufei", "ALoon", "KLi5"
            };

            //translator = this.GetTranslator(a => new string[2]{"L","C"}.Contains(a.Alias));
            translator = this.GetTranslator(a => aliases.Contains(a.Alias));
            Assert.AreEqual("(|(alias=LYufei)(alias=ALoon)(alias=KLi5))", translator.FilterString);
        }

        [Test]
        public void test_translate_number_list_contains_expression() {
            IEnumerable<long> integers = new List<long> {21,33,428};

            //translator = this.GetTranslator(a => new string[2]{"L","C"}.Contains(a.Alias));
            translator = this.GetTranslator(a => integers.Contains(a.Id));
            Assert.AreEqual("(|(id=21)(id=33)(id=428))", translator.FilterString);

            IEnumerable<double> doubles = new List<double> { 21.21, 33.33, 428.11 };

            //translator = this.GetTranslator(a => new string[2]{"L","C"}.Contains(a.Alias));
            translator = this.GetTranslator(a => doubles.Contains(a.Id));
            Assert.AreEqual("(|(id=21.21)(id=33.33)(id=428.11))", translator.FilterString);

            IEnumerable<decimal> decimals = new List<decimal> { 21.21m, 33.33m, 428.11m };

            //translator = this.GetTranslator(a => new string[2]{"L","C"}.Contains(a.Alias));
            translator = this.GetTranslator(a => decimals.Contains(a.Id));
            Assert.AreEqual("(|(id=21.21)(id=33.33)(id=428.11))", translator.FilterString);
        }

        [Test]
        public void test_translate_object_list_contains_expression() {
            IEnumerable<Object> objects = new List<Object>{
                "LYufei", 12, new Exception("EXP"), false
            };

            //translator = this.GetTranslator(a => new string[2]{"L","C"}.Contains(a.Alias));
            translator = this.GetTranslator(a => objects.Contains(a.GIN));
            Assert.AreEqual("(|(EmployeeNumber=LYufei)(EmployeeNumber=12)(EmployeeNumber=System.Exception: EXP)(EmployeeNumber=False))", translator.FilterString);
        }

        [Test]
        public void test_translate_enumerable_contains_expression() {
            IEnumerable<String> aliases = new List<string>{
                "LYufei", "ALoon", "KLi5"
            };

            //translator = this.GetTranslator(a => new string[2]{"L","C"}.Contains(a.Alias));
            translator = this.GetTranslator(a => aliases.Contains(a.Alias));
            Assert.AreEqual("(|(alias=LYufei)(alias=ALoon)(alias=KLi5))", translator.FilterString);
        }

        [Test]
        public void test_translate_inline_array_contains_expression() {
            translator = this.GetTranslator(a => new string[2]{"L","C"}.Contains(a.Alias));
            Assert.AreEqual("(|(alias=L)(alias=C))", translator.FilterString);

            translator = this.GetTranslator(a => new long[2] { 800, 900 }.Contains(a.Id));
            Assert.AreEqual("(|(id=800)(id=900))", translator.FilterString);
        }

        [Test]
        public void test_translate_inline_list_contains_expression() {
            translator = this.GetTranslator(a => new List<string> { "L", "C" }.Contains(a.Alias));
            Assert.AreEqual("(|(alias=L)(alias=C))", translator.FilterString);

            translator = this.GetTranslator(a => new List<long> { 800, 900 }.Contains(a.Id));
            Assert.AreEqual("(|(id=800)(id=900))", translator.FilterString);
        }
        
        [Test]
        public void test_translate_array_property_contains_expression() {
            translator = this.GetTranslator(a => a.Managers.Contains("manager-dn"));
            Assert.AreEqual("(manager=manager-dn)", translator.FilterString);
        }

        [Test]
        public void test_translate_complex_expression() {
            //service.Find(a => a.Alias.StartsWith("ALoo") || a.GIN == "04878765");
            //service.Find(a => a.Alias.StartsWith("ALoon") || a.GIN == "04878765" && a.EmployeeType == "employee");
            string[] aliases = new string[]{
                "lyufei", "aloon", "kli5"
            };
            
            translator = this.GetTranslator(a => aliases.Contains(a.Alias) && a.JobCode == "0000-No Job Code");
            Assert.AreEqual("(&(|(alias=lyufei)(alias=aloon)(alias=kli5))(JobCode=0000-No Job Code))", translator.FilterString);

            translator = this.GetTranslator(a => a.SurName.Contains("Liu") && a.GivenName.StartsWith("Yu"));
            Assert.AreEqual("(&(sn=*Liu*)(GivenName=Yu*))", translator.FilterString);

            translator = this.GetTranslator(a => (a.Alias == "lyufei" || a.Alias == "aloon" || a.Alias == "Kli5") && a.Alias != "lyufei");
            Assert.AreEqual("(&(|(alias=lyufei)(alias=aloon)(alias=Kli5))(!(alias=lyufei)))", translator.FilterString);

            translator = this.GetTranslator(a => a.Alias.StartsWith("LYufei") && a.GIN == "089888" && a.Id <= 20 && (a.Id != 10 || a.GivenName == "Yufei" || !(a.JobCode != "000")));
            Assert.AreEqual("(&(alias=LYufei*)(EmployeeNumber=089888)(id<=20)(|(!(id=10))(GivenName=Yufei)(!(!(JobCode=000)))))", translator.FilterString);
            
            translator = this.GetTranslator(a => (a.ObjectClass == "person" || a.ObjectClass == "user") && (a.Alias == "LYufei" || a.GIN == "098765" || a.City == "Jiangxi"));
            Assert.AreEqual("(&(|(objectClass=person)(objectClass=user))(|(alias=LYufei)(EmployeeNumber=098765)(l=Jiangxi)))", translator.FilterString);

            translator = this.GetTranslator(a => ( (a.JobCode == "888" || a.JobCode == "999") && (a.Id ==12 || a.Id == 22) ) || 
                                                 ( (a.GivenName == "Yufei" && a.SurName == "Liu") || (a.GivenName == "Muxue" && a.SurName == "Jiang") ));
            Assert.AreEqual("(|(&(|(JobCode=888)(JobCode=999))(|(id=12)(id=22)))(&(GivenName=Yufei)(sn=Liu))(&(GivenName=Muxue)(sn=Jiang)))", translator.FilterString);
        }

        [Test]
        public void test_translate_to_string_conversion_on_parameter() {
            translator = this.GetTranslator(a => a.Id.ToString() == "value-string");
            Assert.AreEqual("(id=value-string)", translator.FilterString);
        }

        [Test]
        public void test_translate_contains_without_element() {
            string[] empty_array = new string[0];
            translator = this.GetTranslator(a => empty_array.Contains(a.ObjectClass));
            Assert.AreEqual("", translator.FilterString);
        }

        [Test]
        public void test_translate_contains_without_element2() {
            string[] empty_array = new string[0];

            translator = this.GetTranslator(a => empty_array.Contains(a.ObjectClass) &&
                                                a.CommonName == "Yufei Liu" && 
                                                a.DirectManager == "Mgn" ||
                                                ( a.DisplayName=="display" && a.Alias == "KO"));
            Assert.AreEqual("(|(&(cn=Yufei Liu)(manager=Mgn))(&(DisplayName=display)(alias=KO)))", translator.FilterString);
        }

        /*
        [Test]
        public void test_expression_has_constant_calculation() {
            string value = "Name";
            int value2 = 22;
            translator = this.GetTranslator(a => a.SurName == "Alab " + value || a.Id == 12 + 4 || a.GivenName == "Abc " + "EDF" || a.Id == 22 + value2);
            Assert.AreEqual("(&(|(objectClass=person)(objectClass=user))(|(alias=LYufei)(EmployeeNumber=098765)(l=Jiangxi)))", translator.QueryString);
        }
        */

        private LdapQueryTranslator GetTranslator(Expression<Func<DirectoryAccount, bool>> predicate) {
            return new LdapQueryTranslator(predicate, typeof(DirectoryAccount));
        }

        [Test]
        public void test_not_supported_expression() {
            String query = null;

            Assert.Throws<NotSupportedException>(() => {
                translator = this.GetTranslator(a => a.GIN + a.SurName == "1111");
                query = translator.FilterString;
            },
            "Add expression is not supported, expression: (a.GIN + a.SurName)");

            Assert.Throws<NotSupportedException>(() => {
                translator = this.GetTranslator(a => a.GIN == "1111" + a.SurName);
                query = translator.FilterString;
            },
            "Add expression is not supported, expression: (a.GIN + a.SurName)");

            Assert.Throws<NotSupportedException>(() => {
                translator = this.GetTranslator(a => a.Managers.Contains(a.Alias));
                query = translator.FilterString;
            },
            "lambda expression error: unableto build query from lambda paramter, a value must be supplied expression: a.ManagerDNs.Contains(a.Alias)");
        }
    }
}
