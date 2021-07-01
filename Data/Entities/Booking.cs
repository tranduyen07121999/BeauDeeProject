using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class Booking
    {
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public Guid UserId { get; set; }
        public Guid Id { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
