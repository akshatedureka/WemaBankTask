using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WemaBankTask.Common.Integration;
using WemaBankTask.Entities.DataContext;
using WemaBankTask.Entities.IRepository;
using WemaBankTask.Entities.Model;
using WemaBankTask.Entities.Repository;

namespace WemaBankTask.Services.Test
{
    [TestFixture]
    public class CustomerServiceTest
    {
        private Mock<ICustomerRepository<Customer>> _customerRepo;
        private ApplicationDbContext _dbContext;
        private IThirdPatyIntegration thirdPaty;
        private IMapper _mapper;
        private CustomerService _customerService;
        
        [SetUp]
        public void Setup()
        {
            //_customerRepo = new Mock<ICustomerRepository<Customer>>();
            //_customerService = new CustomerService(
            //    _dbContext.object,)
        }


    }
}
