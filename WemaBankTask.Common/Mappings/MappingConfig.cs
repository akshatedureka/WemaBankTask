using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WemaBankTask.Entities.DTO;
using WemaBankTask.Entities.Model;

namespace WemaBankTask.Common.Mappings
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Customer, GetCustomerDto>().ReverseMap();
        }
        
    }
}
