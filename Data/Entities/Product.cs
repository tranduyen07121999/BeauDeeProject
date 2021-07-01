using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class Product
    {
        public Product()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public DateTime? Expiration { get; set; }

        public virtual Service Service { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
