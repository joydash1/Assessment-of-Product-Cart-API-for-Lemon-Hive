using ProductCart.Infrastructure;
using ProductCart.Infrastructure.Domains;
using ProductCart.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCart.Services
{
    public class ProductRepository : GenericRepository<Products>, IProductRepository
    {
        protected readonly ApplicationDbContext _productDb;

        public ProductRepository(ApplicationDbContext productDb) : base(productDb)
        {
            _productDb = productDb;
        }

        public void Update(Products product)
        => _productDb.Update(product);

        public void UpdateRange(IEnumerable<Products> products)
        => _productDb.UpdateRange(products);
    }
}