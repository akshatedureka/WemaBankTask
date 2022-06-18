using System;
using System.Collections.Generic;
using System.Text;

namespace WemaBankTask.Entities.DTO
{
    public class VerifyCustomerDto
    {
        public string PhoneNumber { get; set; }
        public int OTP { get; set; }
    }
}
