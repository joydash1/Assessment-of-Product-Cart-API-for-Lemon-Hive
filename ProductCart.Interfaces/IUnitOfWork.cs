using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCart.Interfaces
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }

        Task CommitAsync();

        Task RollbackAsync();
    }
}