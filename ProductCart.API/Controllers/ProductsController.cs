using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCart.Infrastructure.Domains;
using ProductCart.Interfaces;
using ProductCart.Services;

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
        [HttpPost]
        [Route("SaveProduct")]
        public async Task<IActionResult> SaveProductsAsync([FromForm] Product model)
        {
            try
            {

                if (model.File != null)
                {
                    var fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Product Images");

                    if (!Directory.Exists(fileDirectory))
                    {
                        Directory.CreateDirectory(fileDirectory);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                    string filePath = Path.Combine(fileDirectory, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(stream);
                    }

                    model.ProductImage = uniqueFileName;
                    model.ProductImage = Path.Combine("Product Images", uniqueFileName);
                }

                await _context.ProductRepository.AddAsync(model);
                await _context.CommitAsync();
                
                return Ok(new
                {
                    Status = true,
                    Message = "Product Save Successfully."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "There is something wrong. please try again."
                });
            }
        }

    }
}