using System;
using System.Collections.Generic;

#nullable disable

namespace Data.ResponseModels
{
    public class ServiceResponse
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Image { get; set; }
        public string Status { get; set; }

    }
}
