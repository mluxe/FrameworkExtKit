using System;
using System.Collections.Generic;
using System.Text;

#if NETCORE
namespace FrameworkExtKit.Core.Data.Entity.Facades
{
    /// <summary>
    /// IJoinEntity, interface for multiple to multiple relationships
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <remarks>.Net Core Only</remarks>
    public interface IJoinEntity<TEntity> {
        TEntity Navigation { get; set; }
    }
}
#endif
