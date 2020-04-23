using FrameworkExtKit.Services.DirectoryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace FrameworkExtKit.Services.DirectoryServices {



    public class MemoryDirectoryAccountService<T> : DirectoryAccountService<T>, IDirectoryAccountService<T> where T: DirectoryAccount {

        private IList<T> memoryDirectoryEntities;

        public MemoryDirectoryAccountService(IEnumerable<T> data) {
            this.memoryDirectoryEntities = data.ToList();
            this.ObjectClass = "*";
        }

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
