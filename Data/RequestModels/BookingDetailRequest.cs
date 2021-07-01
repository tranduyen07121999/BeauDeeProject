using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.RequestModels
{
    public class BookingDetailRequest
    {
        public Guid ServiceId { get; set; }

        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
