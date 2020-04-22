using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

#if NET45
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using FrameworkExtKit.Core.Data.Entity.Validation;
using System.Data.Entity.Migrations;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endif
using Z.EntityFramework.Plus;

namespace FrameworkExtKit.Core.Data.Entity.Repositories
{
    /// <summary>
    /// The Repository Base class which have implemented lots of useful functions
    /// for fetch data from database via DbContext instance
    /// </summary>
    /// <typeparam name="TEntity">The type of entity</typeparam>
    public partial class Repository<TEntity> : IQueryableRepository<TEntity>, IDisposable
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbset;

#if NET45
        protected readonly DbQuery<TEntity> _dbquery;
#elif NETCORE
        protected readonly IQueryable<TEntity> _dbquery;
#endif
        /// <summary>
        /// Construction
        /// </summary>
        /// <param name="context">DbContext</param>
        public Repository(DbContext context) {
            _context = context;
            _dbset = context.Set<TEntity>();
            _dbquery = _dbset;
        }

#if NET45
        protected Repository(DbContext context, DbQuery<TEntity> dbquery) {
#elif NETCORE
        /// <summary>
        /// internal Repository constructor
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="dbquery">db query generated from other db contexts or repositories</param>
        protected Repository(DbContext context, IQueryable<TEntity> dbquery) {
#endif
            _context = context;
            _dbset = context.Set<TEntity>();
            _dbquery = dbquery;
        }

        /// <summary>
        /// Returns a new repository where the change tracker will not track any
        /// of the entities that are returned.
        /// 
        /// Disable change tracker is useful for read-only scenarios because it avoids
        /// overhead of setting up change tracking for each entity instance.
        /// </summary>
        /// <returns>IQueryableRepository</returns>
        public IQueryableRepository<TEntity> AsNoTracking() {
            if(_dbquery != null) {
                return new Repository<TEntity>(_context, _dbquery.AsNoTracking());
            }

            if (_dbset != null) {
                return new Repository<TEntity>(_context, _dbset.AsNoTracking());
            }

            return null;
        }

        /// <summary>
        /// return the DbContext used by the repository
        /// </summary>
        public DbContext DbContext {
            get {
                return _context;
            }
        }

        /// <summary>
        /// add new entity to the repository
        /// </summary>
        /// <param name="entity"></param>
#if NET45
        public virtual TEntity Add(TEntity entity) {
#endif
#if NETCORE
        public virtual EntityEntry<TEntity> Add(TEntity entity) {
#endif
            this.BeforeAdd(entity);
            return _dbset.Add(entity);
        }
#if NETCORE
        public virtual Task<EntityEntry<TEntity>> AddAsync(TEntity entity) {
            this.BeforeAdd(entity);
            return _dbset.AddAsync(entity);
        }
#endif

        /// <summary>
        /// delete a tracked entity
        /// </summary>
        /// <param name="entity">Entity object</param>
        /// <example>
        /// <code>
        /// User user = Repository.GetByAlias("LYufei");
        /// Repository.Delete(user);
        /// </code>
        /// </example>
        public virtual void Delete(TEntity entity) {
            //var entry = _context.Entry(entity);
            //entry.State = System.Data.Entity.EntityState.Deleted;
            _dbset.Remove(entity);
        }

        /// <summary>
        /// execute database deletions base on the provided expression
        /// </summary>
        /// <param name="predicate">the search expression</param>
        /// <returns>number of deleted records</returns>
        /// <example>
        /// <code>
        /// Repository.Delete(user=> user.Id == 12 &amp;&amp; user.Enabled == true);
        /// </code>
        /// </example>
        public int Delete(Expression<Func<TEntity, bool>> predicate) {
            return _dbset.Where(predicate).Delete();
        }

        /// <summary>
        /// Delete everything from the repository
        /// </summary>
        public virtual void DeleteAll() {
            _dbset.Delete();
        }

        /// <summary>
        /// Update a tracked entity
        /// </summary>
        /// <param name="entity">Entity Instance</param>
        /// 
        public virtual void Update(TEntity entity) {
            var entry = _context.Entry(entity);
            this.Update(entry);
        }

        /*
        public virtual int Update(IQueryable<T> query, Expression<Func<T, T>> updateExpression) {
            return _dbset.Update(query, updateExpression);
        }

        public virtual int Update(Expression<Func<T, T>> updateExpression) {
            return _dbset.Update(updateExpression);
        }

        public virtual int Update(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T>> updateExpression) {
            return _dbset.Update(filterExpression, updateExpression);
        }
        */

#if NET45
        private void Update(DbEntityEntry<TEntity> entry) {
#elif NETCORE
        private void Update(EntityEntry<TEntity> entry) {
#endif
            this.BeforeUpdate(entry.Entity);
            entry.State = EntityState.Modified;
            //_dbset.AddOrUpdate(entry.Entity);
            /*
            if (entry.State == EntityState.Detached) {
                _dbset.Attach(entry.Entity);
            }
            entry.State = EntityState.Modified;
            */
        }

        public virtual void AddOrUpdate(TEntity entity) {
            var entry = _context.Entry(entity);

            if (entry.State == EntityState.Detached) {
                this.Add(entity);
            } else {
                this.Update(entry);
            }
        }

        /// <summary>
        /// Save all the entity changes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DbUpdateException"></exception>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="DbEntityValidationException">On Entity Validation failed</exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public int Save() {
            return _context.SaveChanges();
        }

        public Task<int> SaveAsync() {
            return _context.SaveChangesAsync();
        }

#if NET45
        /// <summary>
        /// get the validation errors from repository
        /// </summary>
        public IEnumerable<DbEntityValidationResult> GetValidationErrors() {
            return _context.GetValidationErrors();
        }
#endif


        /*
        public virtual DbSet<T> All
        {
            get {
                return _dbset;
            }
        }
        */

        /// <summary>
        /// callback before a new entity gets added to the repository
        /// </summary>
        /// <param name="entity">Entity instance</param>
        protected virtual void BeforeAdd(TEntity entity) {
            var property = entity.GetType().GetProperty("CreatedAt");
            if (property != null) {
                property.SetValue(entity, DateTime.Now);
            }

            property = entity.GetType().GetProperty("UpdatedAt");
            if (property != null) {
                property.SetValue(entity, DateTime.Now);
            }
        }

        /// <summary>
        /// callback before a existing entity gets updated in the database
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void BeforeUpdate(TEntity entity) {
            var property = entity.GetType().GetProperty("UpdatedAt");
            if (property != null) {
                property.SetValue(entity, DateTime.Now);
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}