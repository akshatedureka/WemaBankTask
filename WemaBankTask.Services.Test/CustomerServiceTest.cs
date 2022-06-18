using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WemaBankTask.Entities.IRepository;
using WemaBankTask.Entities.Model;
using WemaBankTask.Entities.Repository;

namespace WemaBankTask.Services.Test
{
    [TestFixture]
    public class CustomerServiceTest
    {
        private Mock<ICustomerRepository<Customer>> _customerRepo;
        private CustomerService _customerService;

    }
}
