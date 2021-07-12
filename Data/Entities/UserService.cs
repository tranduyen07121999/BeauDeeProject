using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class UserService
    {
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }

        public virtual Service Service { get; set; }
        public virtual User User { get; set; }
    }
}
