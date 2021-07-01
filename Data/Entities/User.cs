using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            UserProductServices = new HashSet<UserProductService>();
            UserStatuses = new HashSet<UserStatus>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal? MinValue { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<UserProductService> UserProductServices { get; set; }
        public virtual ICollection<UserStatus> UserStatuses { get; set; }
    }
}
