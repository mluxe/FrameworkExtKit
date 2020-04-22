using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {
    public abstract class DirectoryService<T> : IDirectoryService<T> where T : DirectoryEntity {
        public virtual string BaseDn { get; set; }
        public virtual string ObjectClass { get; set; }
        public virtual int SizeLimit { get; set; }
        public virtual TimeSpan Timeout { get; set; }

        public T Find(string dn) {
            return this.SingleOrDefault(entity => entity.DistinguishName == dn);
        }

        public virtual T FindById(long id) {
            return this.SingleOrDefault(entity => entity.Id == id);
        }

        public virtual T FindByIdentifier(string identifier) {
            return this.SingleOrDefault(entity => entity.CommonName == identifier ||
                                                    entity.DistinguishName == identifier ||
                                                    entity.UniqueId == identifier ||
                                                    entity.Id.ToString() == identifier);
        }
        public virtual IEnumerable<T> Search(string key) {
            return this.Where(entity => entity.CommonName.Contains(key) ||
                                                    entity.DistinguishName.Contains(key) ||
                                                    entity.UniqueId.Contains(key));
        }

        public virtual IEnumerable<T> FindByName(string name) {
            return this.Where(entity => entity.CommonName == name);
        }

        public virtual T FindByUniqueId(string uniqueId) {
            return this.SingleOrDefault(entity => entity.UniqueId == uniqueId);
        }

        public abstract void Open();
        public abstract void Close();

        public abstract T First(Expression<Func<T, bool>> predicate);
        public abstract T FirstOrDefault(Expression<Func<T, bool>> predicate);
        public abstract T Single(Expression<Func<T, bool>> predicate);
        public abstract T SingleOrDefault(Expression<Func<T, bool>> predicate);
        public abstract IEnumerable<T> Where(Expression<Func<T, bool>> predicate);

        
    }
}
