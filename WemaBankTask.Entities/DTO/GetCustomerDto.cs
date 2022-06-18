using System;
using System.Collections.Generic;
using System.Text;

namespace WemaBankTask.Entities.DTO
{
    public class GetCustomerDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StateOfResidence { get; set; }
        public string LGA { get; set; }
       
    }
}
