using FrameworkExtKit.Core.Data.Entity.Repositories;
using System.Linq;
using System.Linq.Expressions;
#if NET45
using System.Data.Entity.Validation;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
#endif

namespace System.Data.Entity.Migrations {
    public static class DbMigrationsConfigurationExtension {

#if NET45
        public static void Add<TEntity>(this DbMigrationsConfiguration configuration, Expression<Func<TEntity, bool>> exp, DbSet<TEntity> dbSet, TEntity item) 
                    where TEntity : class {
#elif NETCORE
        public static void Add<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> exp, TEntity item)
            where TEntity : class {
#endif

            if (dbSet.AsNoTracking().Any(exp) == false) {
                var property = item.GetType().GetProperty("CreatedAt");
                if (property != null) {
                    property.SetValue(item, DateTime.Now);
                }

                property = item.GetType().GetProperty("UpdatedAt");
                if (property != null) {
                    property.SetValue(item, DateTime.Now);
                }

                dbSet.Add(item);
            }
        }

#if NET45
        public static void Add<TEntity>(this DbMigrationsConfiguration configuration, Expression<Func<TEntity, bool>> exp, Repository<TEntity> repository, TEntity item)
                    where TEntity : class {
#elif NETCORE
        public static void Add<TEntity>(this Repository<TEntity> repository, Expression<Func<TEntity, bool>> exp, TEntity item)
            where TEntity : class {
#endif

            if (repository.Where(exp).Any() == false) {
                repository.Add(item);
            }
        }

#if NET45
        public static int SaveChanges(this DbMigrationsConfiguration configuration, DbContext context) {
            try {
                return context.SaveChanges();
            } catch (DbEntityValidationException ex) {
                string errors = ex.EntityValidationErrors.Count() + " Invalid Entities: \n\n";
                foreach (var validationResult in ex.EntityValidationErrors) {
                    if (validationResult.IsValid == false) {
                        errors += validationResult.Entry.Entity.GetType() + "\n";
                        foreach (var error in validationResult.ValidationErrors) {
                            errors += error.ErrorMessage + "\n";
                        }
                        errors += "\n";
                    }
                }
                DbEntityValidationException e = new DbEntityValidationException(errors, ex.EntityValidationErrors);
                throw e;
            }
        }
#endif

    }
}