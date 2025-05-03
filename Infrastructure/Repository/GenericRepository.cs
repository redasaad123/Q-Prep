using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
            _dbSet = context.Set<T>();

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
             return await _dbSet.FindAsync(id);

        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
             _dbSet.Update(entity);
             return entity;
        }

        public T Delete(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }

        public async Task<List<object>> FindAll(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> Object)
        {
            var query = await _dbSet.Where(predicate).Select(Object).ToListAsync();
            return query;
        }

        public async Task<object> Find( Expression<Func<T, bool>> predicate,Expression<Func<T, object>> Object )
        {
            var query = await _dbSet.Where(predicate).Select(Object).FirstOrDefaultAsync();
            return query;
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate)
        {
            var query = await _dbSet.AnyAsync(predicate);
            return query;
        }


        public async Task<List<object>> FindAll( Expression<Func<T, object>> Object)
        {
            var query = await _dbSet.Select(Object).ToListAsync();
            return query;
        }
        public async Task<List<string>> FindAll(Expression<Func<T, bool>> predicate, Expression<Func<T, string>> Object)
        {
            var query = await _dbSet.Where(predicate).Select(Object).ToListAsync();
            return query;
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            var query =  _dbSet.Where(predicate).FirstOrDefault();

            return query;
        }

        public async Task<object> Mapping(Expression<Func<T, object>> Object)
        {
            var query =  _dbSet.Select(Object);
            return query;
        }

        public async Task<List<T>> FindAll(Expression<Func<T, bool>> predicate)
        {
            var query = await _dbSet.Where(predicate).ToListAsync();

            return query;
        }
    }
}
