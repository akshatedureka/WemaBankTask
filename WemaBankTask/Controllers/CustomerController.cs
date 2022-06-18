using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WemaBankTask.Entities.DTO;
using WemaBankTask.Services.IServices;

namespace WemaBankTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Controller for Customer onboarding
        /// </summary>
        /// <param name="customerService"></param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

       

        /// <summary>
        /// Endpoint to all customers
        /// </summary>
        [HttpGet]
        [Route("GetAllCustomer")]
        [ProducesResponseType(typeof(ResponseModel<List<GetCustomerDto>>), 200)]
        public async Task<IActionResult> GetAllCustomers()
        {
            var allCustomerList = await _customerService.GetAllCustomer();
            if (allCustomerList.HasError)
            {
                return BadRequest(allCustomerList);
            }
            return Ok(allCustomerList);
        }

        /// <summary>
        /// Onboard customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OnboardCustomer")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public async Task<IActionResult> OnboardCustomer(CustomerDto model)
        {
            if (ModelState.IsValid)
            {
                    var response = await _customerService.OnboardCustomer(model);
                    if (response.HasError)
                    {
                        return BadRequest(response);
                    }

                    return Ok(response);
             }
            ModelState.AddModelError("", $"Something went wrong when saving the record");
            return StatusCode(500, ModelState);


        }

        /// <summary>   
        /// Verify customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("VerifyCustomer")]
        [ProducesResponseType(typeof(ResponseModel), 200)]
        public async Task<IActionResult> VerifyCustomer(VerifyCustomerDto model)
        {
            var response = await _customerService.VerifiyCustomer(model);
            if (response.HasError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
