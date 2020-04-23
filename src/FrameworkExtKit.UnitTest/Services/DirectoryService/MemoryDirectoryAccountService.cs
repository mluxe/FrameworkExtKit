using FrameworkExtKit.Services.DirectoryServices;
using FrameworkExtKit.UnitTest.Services.DirectoryService.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;



namespace FrameworkExtKit.UnitTest.Services.DirectoryService {

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
     * By: Yufei Liu <feilfly@gmail.com>
     * Date: 11th June, 2015 @ Gatwick, UK
     * 
     */
    public class MemoryDirectoryAccountService : MemoryDirectoryAccountService<DirectoryAccount>, IDirectoryAccountService {

        public MemoryDirectoryAccountService() : base(MemoryDirectoryAccounts.ToList()) {

        }

        public MemoryDirectoryAccountService(IEnumerable<DirectoryAccount> data) : base(data) {

        }
    }

    public class MemoryDirectoryAccountService<T> : MemoryDirectoryService<T>, IDirectoryAccountService<T> where T: DirectoryAccount {
        
        public MemoryDirectoryAccountService(IEnumerable<T> data): base(data) {
            
        }

        //public virtual IEnumerable<TEntity> Find<TEntity>(Type type, Expression<Func<TEntity, bool>> predicate) {
        //    var p2 = predicate as Expression<Func<T, bool>>;
        //    return memoryDirectoryAccounts.Where(p2.Compile()) as IEnumerable<TEntity>;
        //}

        //public IEnumerable<T> FindTeamMembers(T user) {
        //    var records = this.Where(a => a.ManagerDNs.Contains(user.DistinguishName) && 
        //                                                    a.ObjectClass == this.ObjectClass)
        //                                        .ToList();
        //    return records;
        //}

        //public override void Close() {
        //    // pass, not required
        //}

        //public override void Open() {
        //    // pass, not required
        //}

        //public override T FindByIdentifier(string identifier) {
        //    return this.SingleOrDefault(account => account.Alias == identifier ||
        //                                account.GIN == identifier ||
        //                                //account.DistinguishName == identifier ||
        //                                account.UniqueId == identifier || account.Id.ToString() == identifier);
        //}

        //public T FindByAlias(string alias) {
        //    return this.SingleOrDefault(u => u.Alias == alias);
        //}

        //public T FindByGIN(string gin) {
        //    return this.SingleOrDefault(u => u.GIN == gin);
        //}

        //public T FindDirectManager(T user) {
        //    return this.SingleOrDefault(u => u.Id == user.DirectManagerDirectoryID);
        //}

        //public IEnumerable<T> FindFunctionManagers(T user) {
        //    return this.Where(u => user.ObjectClass == this.ObjectClass && user.ManagerDirectoryIDs.Contains(u.DirectManagerDirectoryID));
        //}
    }
}
