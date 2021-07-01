using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class Status
    {
        public Status()
        {
            UserStatuses = new HashSet<UserStatus>();
        }

        public Guid Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserStatus> UserStatuses { get; set; }
    }
}
