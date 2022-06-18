using System;
using System.Collections.Generic;
using System.Text;

namespace WemaBankTask.Entities.DTO
{
    public class GetBankListResponseModel
    {
        public List<BankModel> Result { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class BankModel
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
    }
}
