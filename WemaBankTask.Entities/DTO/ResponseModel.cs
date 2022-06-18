using System;
using System.Collections.Generic;
using System.Text;

namespace WemaBankTask.Entities.DTO
{
    public class ResponseModel
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }
    }
}
