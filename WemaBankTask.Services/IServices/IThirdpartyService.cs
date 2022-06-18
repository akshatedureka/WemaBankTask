using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WemaBankTask.Entities.DTO;

namespace WemaBankTask.Services.IServices
{
    public interface IThirdpartyService
    {
        Task<ResponseModel<List<GetBankListDto>>> GetBankList();
    }
}
