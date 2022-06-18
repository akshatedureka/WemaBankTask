using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WemaBankTask.Entities.DTO;

namespace WemaBankTask.Services.IServices
{
    public interface ICustomerService
    {
        Task<ResponseModel<List<GetCustomerDto>>> GetAllCustomer();
        Task<ResponseModel> OnboardCustomer(CustomerDto customer);
        Task<ResponseModel> VerifiyCustomer(VerifyCustomerDto varifyCustomerDto);
       
    }
}
