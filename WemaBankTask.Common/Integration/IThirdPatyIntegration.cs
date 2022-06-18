using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WemaBankTask.Entities.DTO;

namespace WemaBankTask.Common.Integration
{
    public  interface IThirdPatyIntegration
    {
        Task<GetBankListResponseModel> GetBankList();
        bool RequestOTP(string phoneNumber);
        bool VerifyOTP(string PhoneNumber, int oTP);
    }
}
