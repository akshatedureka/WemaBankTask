using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WemaBankTask.Entities.DTO;
using WemaBankTask.Services.IServices;

namespace WemaBankTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IThirdpartyService _thirdpartyService;

        /// <summary>
        /// Controller for thirdparty integration
        /// </summary>
        /// <param name="thirdpartyService"></param>
        public BankController(IThirdpartyService thirdpartyService)
        {
            _thirdpartyService = thirdpartyService;
        }
        /// <summary>
        /// Endpoint to get bank list
        /// </summary>
        [HttpGet]
        [Route("GetBankList")]
        [ProducesResponseType(typeof(ResponseModel<List<GetBankListDto>>), 200)]
        public async Task<IActionResult> GetBankList()
        {
            var bankList = await _thirdpartyService.GetBankList();
            if (bankList.HasError)
            {
                return BadRequest(bankList);
            }
            return Ok(bankList);
        }
    }
}
