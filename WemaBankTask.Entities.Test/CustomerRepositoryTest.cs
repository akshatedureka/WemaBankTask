using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WemaBankTask.Entities.DataContext;
using WemaBankTask.Entities.Model;
using WemaBankTask.Entities.Repository;

namespace WemaBankTask.Entities.Test
{
    [TestFixture]
    public class CustomerRepositoryTest
    {
        private Customer customer1;
        private Customer customer2;
        private DbContextOptions<ApplicationDbContext> options;

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_WemaBankTask").Options;
        }
        public CustomerRepositoryTest()
        {
            customer1 = new Customer()
            {
                Id = 1,
                Email = "sawe@gmail.com",
                Password = "222222",
                PhoneNumber = "11111111111",
                StateOfResidence = "Lagos",
                LGA = "Epe"

            };
            customer2 = new Customer()
            {
                Id = 2,
                Email = "sawe22@gmail.com",
                Password = "223332222",
                PhoneNumber = "1d3edd1111111111",
                StateOfResidence = "Ogun",
                LGA = "Ifo"

            };
        }
        [Test]
        public void SaveCustomer_customer1_CheckingValuesFromDatabase()
        {
           
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new CustomerRepository<Customer>(context);
                repository.AddAsyncReturnsId(customer1);

            }
            using (var context = new ApplicationDbContext(options))
            {
                var customerFrmDb = context.Customers.FirstOrDefault();
                Assert.AreEqual(customer1.Id, customerFrmDb.Id);
                Assert.AreEqual(customer1.Email, customerFrmDb.Email);
                Assert.AreEqual(customer1.Id, customerFrmDb.Id);
                Assert.AreEqual(customer1.StateOfResidence, customerFrmDb.StateOfResidence);
                Assert.AreEqual(customer1.LGA, customerFrmDb.LGA);

            }

        }
        //[Test]
        //public  void GetAllCustomer_customer1nd2_CheckBothCustomerFromDataabase()
        //{
        //    var expectedResult = new List<Customer> { customer1, customer2 };
           
        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        context.Database.EnsureCreated();
        //        var repository = new CustomerRepository<Customer>(context);
        //        repository.AddAsyncReturnsId(customer1);
        //        repository.AddAsyncReturnsId(customer2);

        //    }

        //    List<Customer> actualList;
        //    using (var context = new ApplicationDbContext(options))
        //    {
        //        var repository = new CustomerRepository<Customer>(context);
        //        actualList = repository.GetAllAsync();

        //    }
        //    CollectionAssert.AreEqual(expectedResult, actualList, new CustomerCompare());

        //}
        private class CustomerCompare : IComparer
        {
            public int Compare(object x, object y)
            {
                var customer1 = (Customer)x;
                var customer2 = (Customer)y;
                if (customer1.Id != customer2.Id)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            
	
        }

    }
}
