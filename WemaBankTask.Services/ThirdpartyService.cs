using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WemaBankTask.Common.Integration;
using WemaBankTask.Entities.DTO;
using WemaBankTask.Services.IServices;

namespace WemaBankTask.Services
{
    public class ThirdpartyService : IThirdpartyService
    {
        private readonly IThirdPatyIntegration _thirdpartyIntegration;
        public ThirdpartyService(IThirdPatyIntegration thirdpartyIntegration)
        {
            _thirdpartyIntegration = thirdpartyIntegration;
        }
        public async Task<ResponseModel<List<GetBankListDto>>> GetBankList()
        {
            var response = new ResponseModel<List<GetBankListDto>>();
            var bankList = await _thirdpartyIntegration.GetBankList();
            if (bankList.HasError)
            {
                response.HasError = true;
                response.Message = bankList.ErrorMessage;
            }
            else
            {
                var banksListDto = new List<GetBankListDto>();
                foreach (var bank in bankList.Result)
                {
                    var bankDto = new GetBankListDto()
                    {
                        BankCode = bank.BankCode,
                        BankName = bank.BankName
                    };
                    banksListDto.Add(bankDto);
                }

                response.HasError = false;
                response.Message = "Banks retrieved successfully";
                response.Data = banksListDto;
            }

            return response;
        }
    }
}

