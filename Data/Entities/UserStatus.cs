using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class UserStatus
    {
        public Guid StatusId { get; set; }
        public Guid UserId { get; set; }

        public virtual Status Status { get; set; }
        public virtual User User { get; set; }
    }
}
