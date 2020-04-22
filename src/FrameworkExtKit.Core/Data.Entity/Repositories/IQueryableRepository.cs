using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if NET45
using System.Data.Entity;
#endif
#if NETCORE
using Microsoft.EntityFrameworkCore;
#endif

namespace FrameworkExtKit.Core.Data.Entity.Repositories {
    /// <summary>
    /// This is the main repository query interface
    /// </summary>
    /// <typeparam name="TEntity">Entity value</typeparam>
    public interface IQueryableRepository<TEntity> {
        DbContext DbContext { get; }
        TEntity Find(params object[] values);
        Task<TEntity> FindAsync(params object[] values);
        TEntity GetById(long id);
        Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken = default(CancellationToken));
        TEntity GetById(string id);
        Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
        TEntity GetById(int id);
        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken));
        IQueryableRepository<TEntity> Include(string path);
#if NETCORE
        IQueryableRepository<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath);
#endif
        //IQueryableRepository<TEntity> AsNoTracking();
        /** methods from IQuerable<T> **/
        TEntity Aggregate(Expression<Func<TEntity, TEntity, TEntity>> func);
        TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TEntity, TAccumulate>> func);
        TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TEntity, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector);
        IQueryable<TEntity> All();
        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicte);
        int Count();
        Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken));
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        bool Any();
        Task<bool> AnyAsync(CancellationToken cancellationToken = default(CancellationToken));
        bool Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        double Average(Expression<Func<TEntity, int>> selector);
        Task<double> AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default(CancellationToken));
        double Average(Expression<Func<TEntity, long>> selector);
        Task<double> AverageAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default(CancellationToken));
        decimal? Average(Expression<Func<TEntity, decimal?>> selector);
        Task<decimal?> AverageAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        double? Average(Expression<Func<TEntity, double?>> selector);
        Task<double?> AverageAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        float Average(Expression<Func<TEntity, float>> selector);
        Task<float> AverageAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default(CancellationToken));
        double? Average(Expression<Func<TEntity, long?>> selector);
        Task<double?> AverageAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        float? Average(Expression<Func<TEntity, float?>> selector);
        Task<float?> AverageAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        double Average(Expression<Func<TEntity, double>> selector);
        Task<double> AverageAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default(CancellationToken));
        double? Average(Expression<Func<TEntity, int?>> selector);
        Task<double?> AverageAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        decimal Average(Expression<Func<TEntity, decimal>> selector);
        Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken));
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        TEntity Last(Expression<Func<TEntity, bool>> predicate);
#if NETCORE
        Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
#endif
        TEntity Last();
#if NETCORE
        Task<TEntity> LastAsync(CancellationToken cancellationToken = default(CancellationToken));
#endif
        TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate);
#if NETCORE
        Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
#endif
        TEntity LastOrDefault();
#if NETCORE
        Task<TEntity> LastOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken));
#endif
        long LongCount();
        Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken));
        long LongCount(Expression<Func<TEntity, bool>> predicate);
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector);
        Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken));
        TEntity Max();
        Task<TEntity> MaxAsync(CancellationToken cancellationToken = default(CancellationToken));
        TEntity Min();
        Task<TEntity> MinAsync(CancellationToken cancellationToken = default(CancellationToken));
        TResult Min<TResult>(Expression<Func<TEntity, TResult>> selector);
        Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken));
        IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector);
        IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, int, TResult>> selector);
        IQueryable<TResult> SelectMany<TResult>(Expression<Func<TEntity, IEnumerable<TResult>>> selector);
        IQueryable<TResult> SelectMany<TResult>(Expression<Func<TEntity, int, IEnumerable<TResult>>> selector);
        IQueryable<TResult> SelectMany<TCollection, TResult>(Expression<Func<TEntity, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TEntity, TCollection, TResult>> resultSelector);
        IQueryable<TResult> SelectMany<TCollection, TResult>(Expression<Func<TEntity, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TEntity, TCollection, TResult>> resultSelector);
        TEntity Single();
        Task<TEntity> SingleAsync(CancellationToken cancellationToken = default(CancellationToken));
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        TEntity SingleOrDefault();
        Task<TEntity> SingleOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken));
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        int Sum(Expression<Func<TEntity, int>> selector);
        Task<int> SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default(CancellationToken));
        long Sum(Expression<Func<TEntity, long>> selector);
        Task<long> SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default(CancellationToken));
        decimal? Sum(Expression<Func<TEntity, decimal?>> selector);
        Task<decimal?> SumAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        double? Sum(Expression<Func<TEntity, double?>> selector);
        Task<double?> SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        float Sum(Expression<Func<TEntity, float>> selector);
        Task<float> SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default(CancellationToken));
        long? Sum(Expression<Func<TEntity, long?>> selector);
        Task<long?> SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        float? Sum(Expression<Func<TEntity, float?>> selector);
        Task<float?> SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        double Sum(Expression<Func<TEntity, double>> selector);
        Task<double> SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default(CancellationToken));
        int? Sum(Expression<Func<TEntity, int?>> selector);
        Task<int?> SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default(CancellationToken));
        decimal Sum(Expression<Func<TEntity, decimal>> selector);
        Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken));
        IQueryable<TEntity> Take(int count);
#if NETCORE
        IQueryable<TEntity> TakeLast(int count);
#endif
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Where(Expression<Func<TEntity, int, bool>> predicate);

    }
}
