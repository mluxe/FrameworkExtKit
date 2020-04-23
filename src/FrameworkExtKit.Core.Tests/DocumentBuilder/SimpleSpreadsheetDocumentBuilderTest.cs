using FrameworkExtKit.Core.DocumentBuilder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace FrameworkExtKit.Core.Tests.DocumentBuilder {

    [TestFixture]
    [Author("Yufei Liu", "feilfly@gmail.com")]
    public class SimpleSpreadsheetDocumentBuilderTest {
        // SimpleSpreadsheetDocumentBuilder builder;
        String Folder;

        [SetUp]
        public void SetUp() {
            Folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Tests");
            if (!Directory.Exists(Folder)) {
                Directory.CreateDirectory(Folder);
            }

        }

        [TearDown]
        public void TearDown() {
            // builder = null;
        }

        [Test]
        public void test_data_export_should_success() {

            var file_path = Path.Combine(Folder, $"data_export_{DateTime.Now.ToString("yyyyMMdd_HHmmssfff")}.xlsx");
            var builder = new SimpleSpreadsheetDocumentBuilder(file_path);

            List<Object> data = new List<object>();
            var random = new Random();
            for(var i=0; i<20; i++) {
                data.Add(new {
                    Integer_32 = random.Next(),
                    Integer_64 = (long)random.Next(),
                    DateTime_Type = DateTime.Now,
                    Date_Type = DateTime.Now.Date,
                    Text = "This is record " + i,
                   // Boolean_Type = (random.Next() % 2 == 1),
                    Decimal_Type = (decimal)random.Next(),
                    Float_Type = (float)random.NextDouble(),
                    Double_Type = random.NextDouble()
                });
            }

            builder.AddDataToSheet("Sample", data);
            builder.Close();
        }


        private class Human {
            public string Name { get; set; }
            public int Age { get; set; }
            public DateTime BirthDay { get; set; }
        }

        private class Grandparent : Human {
            public Child Amy { get; set; }
            public Child Jimmy { get; set; }
            public GrandChild Tommy { get; set; }
        }

        private class Child : Human {
            public GrandChild Omy { get; set; }
        }


        private class GrandChild : Human {
        }

        [Test]
        public void test_nested_data_property_export_should_success() {

            var file_path = Path.Combine(Folder, $"nested_data_property_export_{DateTime.Now.ToString("yyyyMMdd_HHmmssfff")}.xlsx");
            var builder = new SimpleSpreadsheetDocumentBuilder(file_path);

           
            List<Object> data = new List<object>();
            var random = new Random();
            for (var i = 0; i < 20; i++) {
                Grandparent grandparent = new Grandparent() {
                    Name = "Grand Parent",
                    Age = 101, BirthDay = new DateTime(1920, 1, 1),
                    Amy = new Child() {
                        Name = $"Amy {i}",
                        Age = i, BirthDay = DateTime.Today.AddYears(-i * 5),
                        Omy = new GrandChild() {
                            Name = $"Omy {i}",
                            Age = i, BirthDay = DateTime.Today.AddYears(-i * 2),
                        }
                    },
                    Jimmy = new Child() {
                        Name = $"Jimmy {i}",
                        Age = i, BirthDay = DateTime.Today.AddYears(-i * 3),
                        Omy = null
                    },
                    Tommy = null
                };

                if(i % 2 == 0) {
                    grandparent.Tommy = new GrandChild { Age = 11, BirthDay = DateTime.Today, Name = $"Tommy {i}" };
                }

                data.Add(grandparent);
            }

            builder.AddDataToSheet("Family", data);
            builder.Close();
        }
    }
}
