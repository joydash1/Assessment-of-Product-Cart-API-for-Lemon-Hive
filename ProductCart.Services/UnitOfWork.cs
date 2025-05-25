using ProductCart.Infrastructure;
using ProductCart.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCart.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationDbContext _dbContext;
        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            ProductRepository = new ProductRepository(_dbContext);
        }

        public async Task CommitAsync()
        => await _dbContext.SaveChangesAsync();

        public async Task RollbackAsync()
        => await _dbContext.DisposeAsync();
    }
}