using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ResponseModels
{
    public class BookingDetailResponse
    {
        public Guid ServiceId { get; set; }
        public Guid BookingId { get; set; }

        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }

    }
}
