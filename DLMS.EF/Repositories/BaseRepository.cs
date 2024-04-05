using DLMS.Core.Models;
using DLMS.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DLMS.Core.Constants;

namespace DLMS.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DLMSContext _context;

        public BaseRepository(DLMSContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);

            return entity;
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);

            return entity;
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query.Include(include);
                }
            }

            return await query.SingleOrDefaultAsync(criteria);
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes is not null)
            {
                foreach (var include in includes)
                {
                    query.Include(include);
                }
            }

            return await query.Where(criteria).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take)
        {
            return await _context.Set<T>()
                .Where(criteria)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip,
            int? take, Expression<Func<T, object>> orderBy, string orderByDirection = "ASC")
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (skip.HasValue)
            {
                query.Skip(skip.Value);
            }
            
            if (take.HasValue)
            {
                query.Take(take.Value);
            }

            if (orderBy is not null)
            {
                if (orderByDirection == OrderBy.Ascending)
                {
                    query.OrderBy(orderBy);
                }
                else
                {
                    query.OrderByDescending(orderBy);
                }
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }
        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }
        public void AttachRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
        }
        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().CountAsync(criteria);
        }
    }
}
