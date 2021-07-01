using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class BookingDetail
    {
        public Guid ServiceId { get; set; }
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Product Product { get; set; }
        public virtual Service Service { get; set; }
        public virtual User User { get; set; }
    }
}
