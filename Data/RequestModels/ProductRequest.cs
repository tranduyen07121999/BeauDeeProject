using System;
using System.Collections.Generic;

#nullable disable

namespace Data.RequestModels
{
    public class ProductRequest
    {
        public string Service { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public DateTime Expiration { get; set; }
    }
}
