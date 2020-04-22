using System;
using System.Collections.Generic;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public interface IDirectoryGroupService : IDirectoryGroupService<DirectoryGroup> {
    }

    public partial interface IDirectoryGroupService<T> {
        T FindByAlias(string alias);
    }
}
