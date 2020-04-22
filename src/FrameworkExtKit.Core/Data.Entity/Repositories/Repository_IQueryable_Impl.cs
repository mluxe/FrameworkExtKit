using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

#if NET45
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using FrameworkExtKit.Core.Data.Entity.Validation;
#elif NETCORE
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
#endif
using Z.EntityFramework.Plus;

namespace FrameworkExtKit.Core.Data.Entity.Repositories {
    public partial class Repository<TEntity> : IQueryableRepository<TEntity>, IDisposable where TEntity : class {

        public IQueryableRepository<TEntity> Include(string path) {
            return new Repository<TEntity>(_context, _dbquery.Include(path));
        }

#if NETCORE
        public IQueryableRepository<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) {
            return new Repository<TEntity>(_context, _dbquery.Include(navigationPropertyPath));
        }
#endif

        public TEntity Find(params object[] keyValues) {
            var keys = DbContext.GetKeyNames<TEntity>();

            if(keys.Length != keyValues.Length) {
                throw new ArgumentException($"only expect {keys.Length} key values");
            }

            var parameters = new List<ParameterExpression>();

            var parameter = Expression.Parameter(typeof(TEntity));
            BinaryExpression exp = Expression.Equal(Expression.Property(parameter, keys[0]), Expression.Constant(keyValues[0]));
            for(var i=1; i<keys.Length; i++) {
                var key_exp = Expression.Equal(Expression.Property(parameter, keys[i]), Expression.Constant(keyValues[i]));
                exp = Expression.Add(exp, key_exp);
            }

            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(exp, parameter);
            return _dbquery.Where(lambdaExpression).SingleOrDefault();
            //return _dbset.Find(keyValues);
        }

        public Task<TEntity> FindAsync(params object[] keyValues) {
            var keys = DbContext.GetKeyNames<TEntity>();

            if (keys.Length != keyValues.Length) {
                throw new ArgumentException($"only expect {keys.Length} key values");
            }

            var parameters = new List<ParameterExpression>();

            var parameter = Expression.Parameter(typeof(TEntity));
            BinaryExpression exp = Expression.Equal(Expression.Property(parameter, keys[0]), Expression.Constant(keyValues[0]));
            for (var i = 1; i < keys.Length; i++) {
                var key_exp = Expression.Equal(Expression.Property(parameter, keys[i]), Expression.Constant(keyValues[i]));
                exp = Expression.Add(exp, key_exp);
            }

            var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(exp, parameter);
            return _dbquery.Where(lambdaExpression).SingleOrDefaultAsync();
            //return _dbset.FindAsync(keyValues);
        }

        public virtual TEntity GetById(long id) {
            return Find(id);
        }

        public virtual Task<TEntity> GetByIdAsync(long id, CancellationToken cancellationToken = default(CancellationToken)) {
            return FindAsync(id);
        }

        public virtual TEntity GetById(int id) {
            return Find(id);
        }

        public virtual Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken)) {
            return FindAsync(id);
        }

        public virtual TEntity GetById(string id) {
            return Find(id);
        }

        public virtual Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken)) {
            return FindAsync(id);
        }

        public TEntity Aggregate(Expression<Func<TEntity, TEntity, TEntity>> func) {
            return _dbquery.Aggregate(func);
        }

        public TAccumulate Aggregate<TAccumulate>(TAccumulate seed, Expression<Func<TAccumulate, TEntity, TAccumulate>> func) {
            return _dbquery.Aggregate(seed, func);
        }

        public TResult Aggregate<TAccumulate, TResult>(TAccumulate seed, Expression<Func<TAccumulate, TEntity, TAccumulate>> func, Expression<Func<TAccumulate, TResult>> selector) {
            return _dbquery.Aggregate(seed, func, selector);
        }

        public IQueryable<TEntity> All() {
            return _dbquery.Where(i => (1 == 1));
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicte) {
            return _dbquery.Where(predicte);
        }

        public int Count() {
            return _dbquery.Count();
        }

        public Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.CountAsync(cancellationToken); 
        }

        public int Count(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.Count(predicate);
        }
        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.CountAsync(predicate, cancellationToken);
        }

        public bool Any() {
            return _dbquery.Any();
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AnyAsync(cancellationToken);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.Any(predicate);
        }
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AnyAsync(predicate, cancellationToken);
        }

        public double Average(Expression<Func<TEntity, int>> selector) {
            return _dbquery.Average(selector);
        }
        public Task<double> AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public double Average(Expression<Func<TEntity, long>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<double> AverageAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public decimal? Average(Expression<Func<TEntity, decimal?>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<decimal?> AverageAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public double? Average(Expression<Func<TEntity, double?>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<double?> AverageAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public float Average(Expression<Func<TEntity, float>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<float> AverageAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public double? Average(Expression<Func<TEntity, long?>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<double?> AverageAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public float? Average(Expression<Func<TEntity, float?>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<float?> AverageAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public double Average(Expression<Func<TEntity, double>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<double> AverageAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public double? Average(Expression<Func<TEntity, int?>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<double?> AverageAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public decimal Average(Expression<Func<TEntity, decimal>> selector) {
            return _dbquery.Average(selector);
        }

        public Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.AverageAsync(selector, cancellationToken);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.First(predicate);
        }

        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.FirstAsync(predicate, cancellationToken);
        }


        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public TEntity Last(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.Last(predicate);
        }

#if NETCORE
        public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.LastAsync(predicate, cancellationToken);
        }
#endif

        public TEntity Last() {
            return _dbquery.Last();
        }
#if NETCORE
        public Task<TEntity> LastAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.LastAsync(cancellationToken);
        }
#endif

        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.LastOrDefault(predicate);
        }

#if NETCORE
        public Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.LastOrDefaultAsync(predicate, cancellationToken);
        }
#endif

        public TEntity LastOrDefault() {
            return _dbquery.LastOrDefault();
        }

#if NETCORE
        public Task<TEntity> LastOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.LastOrDefaultAsync(cancellationToken);
        }
#endif

        public long LongCount() {
            return _dbquery.LongCount();
        }

        public Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.LongCountAsync(cancellationToken);
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.LongCount(predicate);
        }

        public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.LongCountAsync(predicate, cancellationToken);
        }

        public TResult Max<TResult>(Expression<Func<TEntity, TResult>> selector) {
            return _dbquery.Max(selector);
        }
        public Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.MaxAsync(selector, cancellationToken);
        }
        public TEntity Max() {
            return _dbquery.Max();
        }
        public Task<TEntity> MaxAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.MaxAsync(cancellationToken);
        }
        public TEntity Min() {
            return _dbquery.Min();
        }
        public Task<TEntity> MinAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.MinAsync(cancellationToken);

        }
        public TResult Min<TResult>(Expression<Func<TEntity, TResult>> selector) {
            return _dbquery.Min(selector);
        }
        public Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.MinAsync(selector, cancellationToken);
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector) {
            return _dbquery.Select(selector);
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, int, TResult>> selector) {
            return _dbquery.Select(selector);
        }

        public IQueryable<TResult> SelectMany<TResult>(Expression<Func<TEntity, IEnumerable<TResult>>> selector) {
            return _dbquery.SelectMany(selector);
        }

        public IQueryable<TResult> SelectMany<TResult>(Expression<Func<TEntity, int, IEnumerable<TResult>>> selector) {
            return _dbquery.SelectMany(selector);
        }

        public IQueryable<TResult> SelectMany<TCollection, TResult>(Expression<Func<TEntity, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TEntity, TCollection, TResult>> resultSelector) {
            return _dbquery.SelectMany(collectionSelector, resultSelector);
        }

        public IQueryable<TResult> SelectMany<TCollection, TResult>(Expression<Func<TEntity, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TEntity, TCollection, TResult>> resultSelector) {
            return _dbquery.SelectMany(collectionSelector, resultSelector);
        }


        public TEntity Single() {
            return _dbquery.Single();
        }

        public Task<TEntity> SingleAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SingleAsync(cancellationToken);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.Single(predicate);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SingleAsync(predicate, cancellationToken);
        }

        public TEntity SingleOrDefault() {
            return _dbquery.SingleOrDefault();
        }

        public Task<TEntity> SingleOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SingleOrDefaultAsync(cancellationToken);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.SingleOrDefault(predicate);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public int Sum(Expression<Func<TEntity, int>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<int> SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public long Sum(Expression<Func<TEntity, long>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<long> SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public decimal? Sum(Expression<Func<TEntity, decimal?>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<decimal?> SumAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public double? Sum(Expression<Func<TEntity, double?>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<double?> SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public float Sum(Expression<Func<TEntity, float>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<float> SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public long? Sum(Expression<Func<TEntity, long?>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<long?> SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public float? Sum(Expression<Func<TEntity, float?>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<float?> SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public double Sum(Expression<Func<TEntity, double>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<double> SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public int? Sum(Expression<Func<TEntity, int?>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<int?> SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public decimal Sum(Expression<Func<TEntity, decimal>> selector) {
            return _dbquery.Sum(selector);
        }

        public Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken)) {
            return _dbquery.SumAsync(selector, cancellationToken);
        }

        public IQueryable<TEntity> Take(int count) {
            return _dbquery.Take(count);
        }

#if NETCORE
        public IQueryable<TEntity> TakeLast(int count) {
            return _dbquery.TakeLast(count);
        }
#endif

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate) {
            return _dbquery.Where(predicate);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, int, bool>> predicate) {
            return _dbquery.Where(predicate);
        }

//        public IQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IQueryable<TFirst> source1, IEnumerable<TSecond> source2, Expression<Func<TFirst, TSecond, TResult>> resultSelector) {
//       }

    }
}
