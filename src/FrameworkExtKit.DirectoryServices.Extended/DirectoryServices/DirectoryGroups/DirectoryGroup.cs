using System;
using System.Collections.Generic;
using System.Text;
#if NET45
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
#endif

namespace FrameworkExtKit.Services.DirectoryServices {
    public partial class DirectoryGroup : DirectoryEntity {
        [DirectoryProperty("employeetype")]
        public virtual string EmployeeType { get; set; }

        [DirectoryProperty("alias")]
        public virtual string Alias { get; set; }
    }
}
