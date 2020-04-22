using FrameworkExtKit.Services.DirectoryServices.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkExtKit.Services.DirectoryServices {

    public partial class LdapDirectoryService<T> : LdapDirectoryService, IDirectoryService<T> where T : DirectoryEntity {

        public virtual T Find(string dn) {
            return this.FindByDn<T>(dn);
        }
        public virtual T FindById(long id) {
            return this.FindById<T>(id);
        }

        public virtual T FindByIdentifier(string identifier) {
            return this.FindByIdentifier<T>(identifier);
        }

        public virtual IEnumerable<T> FindByName(string name) {
            return this.FindByName<T>(name);
        }

        public virtual T FindByUniqueId(string uniqueId) {
            return this.FindByUniqueId<T>(uniqueId);
        }

        public virtual T First(Expression<Func<T, bool>> predicate) {
            return this.First<T>(predicate);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate) {
            return this.FirstOrDefault<T>(predicate);
        }

        public virtual IEnumerable<T> Search(string key) {
            return this.Search<T>(key);
        }

        public virtual T Single(Expression<Func<T, bool>> predicate) {
            return this.Single<T>(predicate);
        }

        public virtual T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            return this.SingleOrDefault<T>(predicate);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate) {
            return this.Where<T>(predicate);
        }
    }

    public partial class LdapDirectoryService : IDisposable {
        public virtual int SizeLimit { get; set; }
        public virtual TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 60);
        public virtual string ObjectClass { get; set; }
        public virtual string BaseDn { get; set; }



        public IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            return this.Where<T>(typeof(T), predicate);
        }

        public IEnumerable<T> Where<T>(Type type, Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            return this.Find<T>(type, predicate);
        }

        public virtual T FindByDn<T>(string dn) where T : DirectoryEntity {
            return this.FindByDn<T>(typeof(T), dn);
        }

        public virtual T FindByDn<T>(Type type,string dn) where T : DirectoryEntity {
            IEnumerable<T> entities = this.Find<T>(type, dn);
            return entities.SingleOrDefault();
        }

        public virtual IEnumerable<T> Find<T>(Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            return this.Find<T>(typeof(T), predicate);
        }

        public virtual IEnumerable<T> Find<T>(Type type, Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            LdapQueryTranslator translator = new LdapQueryTranslator(predicate, type);
            var filter = translator.FilterString;

            if (String.IsNullOrEmpty(filter)) {
                return new T[0];
            }
            return this.Find<T>(type, filter);
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            return FirstOrDefault<T>(typeof(T), predicate);
        }

        public T FirstOrDefault<T>(Type type, Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            IEnumerable<T> entities = this.Find<T>(type, predicate);
            return entities.FirstOrDefault();
        }

        public T First<T>(Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            return First<T>(typeof(T), predicate);
        }

        public T First<T>(Type type, Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            IEnumerable<T> entities = this.Find<T>(type, predicate);
            return entities.First();
        }


        public T Single<T>(Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            return Single<T>(typeof(T), predicate);
        }

        public T Single<T>(Type type, Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            IEnumerable<T> entities = this.Find<T>(type, predicate);
            return entities.Single();
        }

        public T SingleOrDefault<T>(Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            return SingleOrDefault<T>(typeof(T), predicate);
        }

        public T SingleOrDefault<T>(Type type, Expression<Func<T, bool>> predicate) where T : DirectoryEntity {
            IEnumerable<T> entities = this.Find<T>(type, predicate);
            return entities.SingleOrDefault();
        }

        public T FindById<T>(long id) where T : DirectoryEntity {
            return this.FindById<T>(typeof(T), id);
        }

        public T FindById<T>(Type type, long id) where T : DirectoryEntity {
            return this.SingleOrDefault<T>(type, entity => entity.Id == id);
        }

        public T FindByUniqueId<T>(string uid) where T : DirectoryEntity {
            return this.FindByUniqueId<T>(typeof(T), uid);
        }

        public T FindByUniqueId<T>(Type type, string uid) where T : DirectoryEntity {
            return this.SingleOrDefault<T>(type, entity => entity.UniqueId == uid);
        }

        public IEnumerable<T> FindByName<T>(string name) where T : DirectoryEntity {
            return this.FindByName<T>(typeof(T), name);
        }

        public virtual IEnumerable<T> FindByName<T>(Type type, string name) where T : DirectoryEntity {
            return this.Find<T>(type, entity => entity.CommonName == name);
        }

        public T FindByIdentifier<T>(string identifier) where T : DirectoryEntity {
            return this.FindByIdentifier<T>(typeof(T), identifier);
        }

        public virtual T FindByIdentifier<T>(Type type, string identifier) where T : DirectoryEntity {
            return this.SingleOrDefault<T>(type, entity => entity.CommonName == identifier ||
                                                    entity.DistinguishName == identifier ||
                                                    entity.UniqueId == identifier ||
                                                    entity.Id.ToString() == identifier);
        }

        public IEnumerable<T> Search<T>(string key) where T : DirectoryEntity {
            return this.Search<T>(typeof(T), key);
        }

        public virtual IEnumerable<T> Search<T>(Type type, string key) where T : DirectoryEntity {
            return this.Where<T>(type, entity => entity.CommonName.Contains(key) ||
                                                    entity.DistinguishName.Contains(key) ||
                                                    entity.UniqueId.Contains(key));
        }
    }
}
