using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Data.ResponseModels
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string UId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal MinValue { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
    }
}
