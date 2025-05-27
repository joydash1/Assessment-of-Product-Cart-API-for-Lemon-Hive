using Microsoft.EntityFrameworkCore;
using ProductCart.Infrastructure;
using ProductCart.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductCart.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
           => _dbContext.Add(entity);

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _dbContext.AddAsync(entity, cancellationToken);

        public void AddRange(IEnumerable<T> entities)
            => _dbContext.AddRange(entities);

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => await _dbContext.AddRangeAsync(entities, cancellationToken);

        public T Get(Expression<Func<T, bool>> expression)
            => _dbSet.FirstOrDefault(expression);

        public IEnumerable<T> GetAll()
            => _dbSet.AsEnumerable();

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression)
            => _dbSet.Where(expression).AsEnumerable();

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _dbSet.ToListAsync(cancellationToken);

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
            => await _dbSet.Where(expression).ToListAsync(cancellationToken);

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(expression, cancellationToken);

        public void Remove(T entity)
            => _dbContext.Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
            => _dbContext.RemoveRange(entities);

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}