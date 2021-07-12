using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class Service
    {
        public Service()
        {
            BookingDetails = new HashSet<BookingDetail>();
            Products = new HashSet<Product>();
            UserServices = new HashSet<UserService>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }

        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<UserService> UserServices { get; set; }
    }
}
