using FrameworkExtKit.Core.NetCore.Tests.Fixtures;
using FrameworkExtKit.Core.NetCore.Tests.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameworkExtKit.Core.NetCore.Tests.Data.Entity
{
    [Author("Yufei Liu", "feilfly@gmail.com")]
    public class ManyToManyRelationshipTest
    {
        TestDbContext dbContext;
        [SetUp]
        public void SetUp() {
            string[] args = new string[0];

            TestDbContextFactory factory = new TestDbContextFactory();

            this.dbContext = factory.CreateDbContext(args);
            TestDbInitializer.Seed(this.dbContext);
        }

        [TearDown]
        public void TearDown() {
            this.dbContext = null;
        }

        [Test]
        public void test_query_many_to_many_relationship_data() {
            var mappings = dbContext.ManyToManyEntityABMappings.AsNoTracking().ToList();

            Assert.AreEqual(3, mappings.Count);

            var entityAs = dbContext.ManyToManyEntityAs.AsNoTracking().ToList();
            Assert.AreEqual(10, entityAs.Count);

            var entityBs = dbContext.ManyToManyEntityBs.AsNoTracking().ToList();
            Assert.AreEqual(2, entityBs.Count);

            var entityB1 = dbContext.ManyToManyEntityBs.Include("ManyToManyEntityABMappings.EntityA")
                                .AsNoTracking()
                                .Where(b => b.Id == 1).First();
            Assert.AreEqual(3, entityB1.ManyToManyEntityAs.Count);

            var entityA1 = dbContext.ManyToManyEntityAs.Include("ManyToManyEntityABMappings.EntityB")
                               .AsNoTracking()
                               .Where(a => a.Id == 1).First();
            Assert.AreEqual(1, entityA1.ManyToManyEntityBs.Count);
        }

        [Test]
        [Ignore("test failed, fixed is required")]
        public void test_query_entity_from_mapping_relationship() {
            var group1Entities = dbContext.ManyToManyEntityBs.Include("ManyToManyEntityABMappings.EntityA")
                                            .AsNoTracking()
                                            .Where(a => a.ManyToManyEntityAs.Any(b => b.Id == 1))
                                            .ToList();
            Assert.AreEqual(3, group1Entities.Count);
        }
    }
}
