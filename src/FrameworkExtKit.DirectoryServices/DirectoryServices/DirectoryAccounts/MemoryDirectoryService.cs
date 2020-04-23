using FrameworkExtKit.Services.DirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace FrameworkExtKit.Services.DirectoryServices {

    /**
     *
     * The MemoryDirectoryService is developed as a utility to help with the unit testing
     * so we do not have to change the unit testing code when LDAP reporting structure changes
     * 
     * To use the Directory Service, you can 
     * 1. add new configuration to the config file
     * <add key="DirectoryAccountService.ClassName" value="FrameworkExtKit.Tests.Services.DirectoryService.MemoryDirectoryService, FrameworkExtKit.UnitTest" />
     * 
     * 2. then use this method to create a directory instance
     *      DirectoryAccountService.GetUserDirectoryInstance()
     * 
     * By: Yufei Liu <yliu@leyun.co.uk>
     * Date: 11th June, 2015 @ Gatwick, UK
     * 
     */
    public partial class MemoryDirectoryService<T> : DirectoryService<T> where T:DirectoryEntity {
        private IList<T> memoryDirectoryEntities;

        public MemoryDirectoryService(IEnumerable<T> data) {
            this.memoryDirectoryEntities = data.ToList();
            this.ObjectClass = "*";
        }

        //public override IEnumerable<TEntity> Find<TEntity>(Type type, Expression<Func<TEntity, bool>> predicate) {
        //    var p2 = predicate as Expression<Func<T, bool>>;
        //    var results = memoryDirectoryAccounts.Where(p2.Compile()).ToList();
        //    return results as IEnumerable<TEntity>;
        //}

        public override void Close() {
            // pass, not required
        }

        public override void Open() {
            // pass, not required
        }

        public override IEnumerable<T> Where(Expression<Func<T, bool>> predicate) {
            //var p2 = predicate as Expression<Func<T, bool>>;
            return memoryDirectoryEntities.Where(predicate.Compile());
        }

        public override T Single(Expression<Func<T, bool>> predicate) {
            return memoryDirectoryEntities.Single(predicate.Compile());
        }

        public override T SingleOrDefault(Expression<Func<T, bool>> predicate) {
            return memoryDirectoryEntities.SingleOrDefault(predicate.Compile());
        }

        public override T First(Expression<Func<T, bool>> predicate) {
            return memoryDirectoryEntities.First(predicate.Compile());
        }

        public override T FirstOrDefault(Expression<Func<T, bool>> predicate) {
            return memoryDirectoryEntities.FirstOrDefault(predicate.Compile());
        }
    }
}
