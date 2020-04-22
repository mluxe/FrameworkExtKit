#if NETCORE
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {
    public interface IDirectoryServiceFactory {

        IDirectoryAccountService GetDirectoryAccountService();
        IDirectoryDistributionListService GetDirectoryDistributionListService();
        IDirectoryGroupService GetDirectoryGroupService();
        IDirectoryRoleAccountService GetDirectoryRoleAccountService();
        IDirectoryService<TDirectoryEntity> GetDirectoryService<TDirectoryEntity>() where TDirectoryEntity : DirectoryEntity;
    }
}
#endif