using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCart.Interfaces;

namespace ProductCart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _context;

        public ProductsController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetProductList")]
        public async Task<IActionResult> GetAllProductsListAsync()
        {
            var productList = await _context.ProductRepository.GetAllAsync();
            return Ok(new
            {
                Data = productList
            });
        }
    }
}