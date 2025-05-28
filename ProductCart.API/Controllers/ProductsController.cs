using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCart.Infrastructure.Domains;
using ProductCart.Interfaces;
using ProductCart.Services;
using ProductCart.Shared.Helpers;

namespace ProductCart.API.Controllers
{
    [Route("[controller]")]
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
        public async Task<IActionResult> GetAllProductsListAsync(string? ProductName = "", int page = 1, int pageSize = 8)
        {
            var query = _context.ProductRepository.Query();

            if (!string.IsNullOrWhiteSpace(ProductName))
                query = query.Where(p => p.ProductName.Contains(ProductName));

            var totalCount = await query.CountAsync();

            var currentDate = DateTime.Now;

            var products = await query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.Id,
                    p.ProductName,
                    p.ProductPrice,
                    p.ProductSlug,
                    p.ProductImage,
                    p.DiscountStartDate,
                    p.DiscountEndDate,
                    ProductDiscountedPrice =
                        (p.DiscountStartDate <= currentDate && p.DiscountEndDate >= currentDate)
                            ? Math.Round(p.ProductPrice * 0.75m, 2)
                            : (decimal?)null
                })
                .ToListAsync();

            return Ok(new
            {
                Data = new
                {
                    Items = products,
                    TotalCount = totalCount
                }
            });
        }

        [HttpPost]
        [Route("SaveProduct")]
        public async Task<IActionResult> SaveProductsAsync([FromForm] Products model)
        {
            try
            {
                if (model.File != null)
                {
                    var fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ProductImages");

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
                    model.ProductImage = Path.Combine("ProductImages", uniqueFileName);
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