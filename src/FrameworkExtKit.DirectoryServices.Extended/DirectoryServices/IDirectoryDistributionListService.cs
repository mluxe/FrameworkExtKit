using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {
    public interface IDirectoryDistributionListService : IDirectoryDistributionListService<DirectoryDistributionList> {
    }

    public partial interface IDirectoryDistributionListService<T> {
        // TDirectoryAccount FindManager<TDirectoryAccount>(T distributionList) where TDirectoryAccount:DirectoryAccount;
        IEnumerable<T> FindByManager(string managerIdentifier);
        T FindByAlias(string alias);
    }
}
