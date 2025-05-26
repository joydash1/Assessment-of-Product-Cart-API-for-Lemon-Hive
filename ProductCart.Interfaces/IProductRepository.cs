using ProductCart.Infrastructure.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCart.Interfaces
{
    public interface IProductRepository : IGenericRepository<Products>
    {
        void Update(Products product);

        void UpdateRange(IEnumerable<Products> products);
    }
}