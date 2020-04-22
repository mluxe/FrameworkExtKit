using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkExtKit.Services.DirectoryServices {
    public interface IDirectoryEntityService : IDirectoryService<DirectoryEntity> {

    }

    public interface IDirectoryService<TDirectoryEntity> : IDirectoryService where TDirectoryEntity : DirectoryEntity {

        TDirectoryEntity Find(string dn);
        TDirectoryEntity FindById(long id);
        TDirectoryEntity FindByUniqueId(string uniqueId);
        TDirectoryEntity FindByIdentifier(string identifier);
        IEnumerable<TDirectoryEntity> FindByName(string name);
        IEnumerable<TDirectoryEntity> Search(string key);
        //        IEnumerable<TDirectoryEntity> Search(string objectType, string key);
        IEnumerable<TDirectoryEntity> Where(Expression<Func<TDirectoryEntity, bool>> predicate);
        TDirectoryEntity Single(Expression<Func<TDirectoryEntity, bool>> predicate);
        TDirectoryEntity SingleOrDefault(Expression<Func<TDirectoryEntity, bool>> predicate);
        TDirectoryEntity First(Expression<Func<TDirectoryEntity, bool>> predicate);
        TDirectoryEntity FirstOrDefault(Expression<Func<TDirectoryEntity, bool>> predicate);
        

    }

    public interface IDirectoryService {
        string BaseDn { get; set; }
        string ObjectClass { get; set; }
        int SizeLimit { get; set; }
        TimeSpan Timeout { get; set; }
        void Open();
        void Close();
    }
}
