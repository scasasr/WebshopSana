using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Domain.Models
{
    public class ProductsPaginationResponse
    {
        public bool HaveItems { get; set; }
        public int Items { get; set; }
        public int TotalPages { get; set; }
        public List<Product> Data { get; set; }
    }
}
