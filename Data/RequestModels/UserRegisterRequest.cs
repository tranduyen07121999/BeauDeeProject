using System;
using System.Collections.Generic;
#nullable disable

namespace Data.RequestModels
{
    public class UserRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string Image { get; set; }
    }
}
