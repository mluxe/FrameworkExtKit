using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FrameworkExtKit.Services.DirectoryServices {

    public abstract partial class DirectoryGroupService<T> {

        public virtual T FindByAlias(string alias) {
            return this.SingleOrDefault(entity => entity.Alias == alias);
        }

    }
}
