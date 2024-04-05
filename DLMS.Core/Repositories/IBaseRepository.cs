using DLMS.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DLMS.Core.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T> AddAsync(T entity);
        T Update(T entity);
        void Delete(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy, string orderByDirection = OrderBy.Ascending);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
    }
}
