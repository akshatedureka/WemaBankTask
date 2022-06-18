using System;
using System.Collections.Generic;
using System.Text;

namespace WemaBankTask.Entities.DTO
{
    public class CustomerDto
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StateOfResidence { get; set; }
        public string LGA { get; set; }
    }
}
