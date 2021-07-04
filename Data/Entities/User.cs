using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class User
    {
        public User()
        {
            BookingDetails = new HashSet<BookingDetail>();
            Bookings = new HashSet<Booking>();
            UserRoles = new HashSet<UserRole>();
            UserStatuses = new HashSet<UserStatus>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal MinValue { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string Image { get; set; }

        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserStatus> UserStatuses { get; set; }
    }
}
