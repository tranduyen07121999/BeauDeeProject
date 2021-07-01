using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Entities
{
    public partial class UserProductService
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
