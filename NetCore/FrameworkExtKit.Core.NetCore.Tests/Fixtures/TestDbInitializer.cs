using FrameworkExtKit.Core.NetCore.Tests.Models;
using FrameworkExtKit.Core.NetCore.Tests.Models.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z.EntityFramework.Plus;

namespace FrameworkExtKit.Core.NetCore.Tests.Fixtures {
    public static class TestDbInitializer {

        public static void Seed(TestDbContext dbContext) {

            if (dbContext.ManyToManyEntityABMappings.Any()) {
                dbContext.ManyToManyEntityABMappings.Where(m => true).Delete();
                dbContext.ManyToManyEntityAs.Where(m => true).Delete();
                dbContext.ManyToManyEntityBs.Where(m => true).Delete();
                
                dbContext.SaveChanges();
            }

            for(var i=1; i<11; i++) {
                dbContext.ManyToManyEntityAs.Add(new ManyToManyEntityA { Id = i, Name = "Item " + i });
            }

            dbContext.SaveChanges();


            List<ManyToManyEntityB> entityBs = new List<ManyToManyEntityB>() {
                new ManyToManyEntityB { Id = 1, Name = "First Group", Category = "Category 1" },
                new ManyToManyEntityB { Id = 2, Name = "Second Group", Category = "Category 2" }
            };

            foreach(var entity in entityBs) {
                dbContext.ManyToManyEntityBs.Add(entity);
            }

            dbContext.SaveChanges();


            List<ManyToManyEntityABMapping> mappings = new List<ManyToManyEntityABMapping>() {
                new ManyToManyEntityABMapping { ManyToManyEntityAId = 1, ManyToManyEntityBId = 1},
                new ManyToManyEntityABMapping { ManyToManyEntityAId = 2, ManyToManyEntityBId = 1},
                new ManyToManyEntityABMapping { ManyToManyEntityAId = 5, ManyToManyEntityBId = 1},
            };

            foreach(var mapping in mappings) {
                dbContext.ManyToManyEntityABMappings.Add(mapping);
            }

            dbContext.SaveChanges();
        }
    }
}
