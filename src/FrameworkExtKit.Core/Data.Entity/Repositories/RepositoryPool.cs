using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETCORE
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
#endif

namespace FrameworkExtKit.Core.Data.Entity.Repositories {
    /// <summary>
    /// The collection of repositorys
    /// </summary>
    /// <typeparam name="TContext">DbContext subclass</typeparam>
    public abstract class RepositoryPool<TContext> : IDisposable where TContext : DbContext{

        protected TContext _dbContext;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context">DbContext</param>
        public RepositoryPool(TContext context) {
            _dbContext = context;
        }

#if NET45
        /// <summary>
        /// get the underline database instance
        /// </summary>
        public Database Database {
#elif NETCORE
        /// <summary>
        /// get the underline database instance
        /// </summary>
        public DatabaseFacade Database {
#endif
            get {
                return _dbContext.Database;
            }
        }

        /// <summary>
        /// get the Db Context instance
        /// </summary>
        public TContext DbContext {
            get {
                return _dbContext;
            }
        }

        /// <summary>
        /// save the pending changes on all repositories
        /// </summary>
        /// <returns></returns>
        public int Save() {
#if DEBUG
#if NET45
            try {
                return _dbContext.SaveChanges();
            }catch(System.Data.Entity.Validation.DbEntityValidationException e) {
                var errors = e.EntityValidationErrors;
                StringBuilder msg = new StringBuilder();

                foreach(var err in errors) {
                    var errors2 = err.ValidationErrors;
                    msg.AppendLine(err.GetType().FullName);
                    foreach(var err2 in errors2) {
                        msg.Append("\t");
                        msg.AppendLine(err2.ErrorMessage);
                    }
                }
                throw new Exception(msg.ToString());
            }
#elif NETCORE
            return _dbContext.SaveChanges();
#endif
#else
            return _dbContext.SaveChanges();
#endif
        }

        /// <summary>
        /// Async save
        /// </summary>
        /// <returns></returns>
        public Task<int> SaveAsync() {
            return _dbContext.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    _dbContext.Dispose();
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
