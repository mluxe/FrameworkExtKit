using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.IO;
using System.Text;

namespace FrameworkExtKit.Core.NetCore.Tests.Models.Contexts
{

    public class TestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext> {
        public TestDbContext CreateDbContext(string[] args) {
            var dir = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
            .SetBasePath(dir)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            TestDbContext dbContext = new TestDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }
    }
    public class TestDbContext : DbContext {
        public TestDbContext(DbContextOptions options)
            : base(options) {
        }

        public DbSet<ManyToManyEntityA> ManyToManyEntityAs { get; set; }
        public DbSet<ManyToManyEntityB> ManyToManyEntityBs { get; set; }
        public DbSet<ManyToManyEntityABMapping> ManyToManyEntityABMappings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<ManyToManyEntityABMapping>()
                        .HasKey(m => new { m.ManyToManyEntityAId, m.ManyToManyEntityBId });

            modelBuilder.Entity<ManyToManyEntityABMapping>()
                        .HasOne(m => m.EntityA)
                        .WithMany("ManyToManyEntityABMappings")
                        .HasForeignKey(m => m.ManyToManyEntityAId);

            modelBuilder.Entity<ManyToManyEntityABMapping>()
                        .HasOne(m => m.EntityB)
                        .WithMany("ManyToManyEntityABMappings")
                        .HasForeignKey(m => m.ManyToManyEntityBId);

            modelBuilder.Entity<ManyToManyEntityA>()
                   .Property(c => c.Id)
                   .ValueGeneratedNever()
                   .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            modelBuilder.Entity<ManyToManyEntityB>()
                   .Property(c => c.Id)
                   .ValueGeneratedNever()
                   .HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
