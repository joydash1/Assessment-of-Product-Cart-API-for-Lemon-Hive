using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductCart.Infrastructure.Domains;
using ProductCart.Interfaces;
using ProductCart.Services;
using ProductCart.Shared.Helpers;

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
        public async Task<IActionResult> GetAllProductsListAsync(string? ProductName = "")
        {
            var productList = await _context.ProductRepository.GetAllAsync();

            var currentDate = CommonHelpers.GetBangladeshTimeZone(DateTime.UtcNow);

            var filteredList = string.IsNullOrWhiteSpace(ProductName)
                ? productList
                : productList.Where(p => p.ProductName.Contains(ProductName, StringComparison.OrdinalIgnoreCase));

            var result = filteredList.Select(p => new
            {
                p.Id,
                p.ProductName,
                p.ProductPrice,
                p.ProductSlug,
                p.ProductImage,
                p.DiscountStartDate,
                p.DiscountEndDate,
                ProductDiscountedPrice = (
                    p.DiscountStartDate.HasValue && p.DiscountEndDate.HasValue &&
                    CommonHelpers.GetBangladeshTimeZone(p.DiscountStartDate.Value) <= currentDate &&
                    CommonHelpers.GetBangladeshTimeZone(p.DiscountEndDate.Value) >= currentDate
                )
                ? Math.Round(p.ProductPrice * 0.75m, 2)
                : (decimal?)null
            });
            return Ok(new { Data = result });
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
                if (model.DiscountStartDate.HasValue)
                    model.DiscountStartDate = CommonHelpers.GetBangladeshTimeZone(model.DiscountStartDate.Value);

                if (model.DiscountEndDate.HasValue)
                    model.DiscountEndDate = CommonHelpers.GetBangladeshTimeZone(model.DiscountEndDate.Value);



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