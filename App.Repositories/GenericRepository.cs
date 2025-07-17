using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories
{
    public class GenericRepository<T> (AppDbContext context) : IGenericRepository<T> where T: class
    {
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async ValueTask<T> AddAsync(T enitiy)
        {
            await _dbSet.AddAsync(enitiy);
            return enitiy;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> GetAll() => _dbSet.AsQueryable().AsNoTracking();  

        public ValueTask<T?> GetByIdAsync(int id)
        {
            return _dbSet.FindAsync(id);
        }

        public void Update(T enitiy)
        {
            _dbSet.Update(enitiy);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking();
        }

    }
}
