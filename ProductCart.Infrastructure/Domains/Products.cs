using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCart.Infrastructure.Domains
{
    public class Products
    {
        [Key]
        public int Id { get; set; }

        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductSlug { get; set; }
        public string? ProductImage { get; set; }

        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }

        [NotMapped]
        public decimal? ProductDiscountedPrice { get; set; }
    }
}