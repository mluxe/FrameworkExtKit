using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Configuration;
#if NETCORE
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
#endif

namespace FrameworkExtKit.Services.DirectoryServices.DB {
    
    public partial class DbDirectoryService<T> : DirectoryService<T> where T : DirectoryEntity {

        String connectionName;
        DbDirectoryContext<T> dbContext;

        public override T FindById(long id) {
            return this.dbContext.DirectoryEntities.Single(d => d.Id == id);
        }

        public override T FindByIdentifier(string identifier) {
            return this.dbContext.DirectoryEntities.SingleOrDefault(entity => entity.CommonName == identifier ||
                                                    entity.DistinguishName == identifier ||
                                                    entity.UniqueId == identifier ||
                                                    entity.Id.ToString() == identifier);
        }

        public override IEnumerable<T> FindByName(string name) {
            return this.dbContext.DirectoryEntities.Where(entity => entity.CommonName == name).ToList();
        }

        public override T FindByUniqueId(string uniqueId) {
            return this.dbContext.DirectoryEntities.SingleOrDefault(entity => entity.UniqueId == uniqueId);
        }

        public override T First(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.First(predicate);
        }

        public override T FirstOrDefault(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.FirstOrDefault(predicate);
        }

        public override IEnumerable<T> Search(string key) {
            return this.dbContext.DirectoryEntities.Where(entity => entity.CommonName.Contains(key) ||
                                                    entity.DistinguishName.Contains(key) ||
                                                    entity.UniqueId.Contains(key)).ToList();
        }

        public override T Single(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.Single(predicate);
        }

        public override T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.Single(predicate);
        }

        public override IEnumerable<T> Where(Expression<Func<T, bool>> predicate) {
            return this.dbContext.DirectoryEntities.Where(predicate).ToList();
        }
        
    }
}
