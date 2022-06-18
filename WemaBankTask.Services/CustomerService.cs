using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WemaBankTask.Common;
using WemaBankTask.Common.Integration;
using WemaBankTask.Entities.DataContext;
using WemaBankTask.Entities.DTO;
using WemaBankTask.Entities.IRepository;
using WemaBankTask.Entities.Model;
using WemaBankTask.Entities.Repository;
using WemaBankTask.Services.IServices;

namespace WemaBankTask.Services
{
    public class CustomerService: BaseManager<Customer>, ICustomerService
    {
        private readonly IThirdPatyIntegration _thirdpartyIntegration;
        private readonly IMapper _mapper;
        //IConfigurationRoot configuration = new ConfigurationBuilder().Build();
        public static LocationsDto locationsDto;
        public CustomerService(ApplicationDbContext Dbcontext, IThirdPatyIntegration thirdpartyIntegration, IMapper mapper) : base(Dbcontext)
        {
         
            _mapper = mapper;
            _thirdpartyIntegration = thirdpartyIntegration;
            locationsDto = new LocationsDto()
            {
                States = new List<State>()
                {
                    new State()
                    {
                        StateId = 1,
                        StateName = "Lagos",
                        LGAs = new List<LGA>()
                        {
                            new LGA()
                            {
                                LGAId = 1,
                                LGAName = "Epe"
                            },
                            new LGA()
                            {
                                LGAId = 2,
                                LGAName = "Kosofe"
                            },
                            new LGA()
                            {
                                LGAId = 2,
                                LGAName = "Alimosho"
                            }
                        }
                    },
                     new State()
                    {
                        StateId = 2,
                        StateName = "Ogun",
                        LGAs = new List<LGA>()
                        {
                            new LGA()
                            {
                                LGAId = 1,
                                LGAName = "Ifo"
                            },
                            new LGA()
                            {
                                LGAId = 2,
                                LGAName = "Obafemi-Owode"
                            },
                            new LGA()
                            {
                                LGAId = 2,
                                LGAName = "Abeokuta North"
                            }
                        }
                    },
                }
            };
        }

        public async Task<ResponseModel<List<GetCustomerDto>>> GetAllCustomer()
        {
            var response = new ResponseModel<List<GetCustomerDto>>();
            response.HasError = false;
            var allCustomers = (await this.GetAllAsync()).ToList();
            if (allCustomers.Count > 0)
            {
                var allCustomersDto = _mapper.Map<List<GetCustomerDto>>(allCustomers);
                response.Data = allCustomersDto;
                response.Message = $"{allCustomers.Count} record(s) found";
            }
            else
            {
                response.Message = "No record found";
            }
            return response;
        }
        public async Task<ResponseModel> OnboardCustomer(CustomerDto customerDto)
        {
            var response = new ResponseModel();

           
            State state;
            LGA lga;
            state = locationsDto.States.Where(x => x.StateName.ToLower() == customerDto.StateOfResidence.ToLower()).FirstOrDefault();
            if (state == null)
            {
                response.HasError = true;
                response.Message = "Invalid state enterd.";
                return response;
            }
            else
            {
                lga = state.LGAs.Where(x => x.LGAName.ToLower() == customerDto.LGA.ToLower()).FirstOrDefault();
                if (lga == null)
                {
                    response.HasError = true;
                    response.Message = "Invalid LGA enterd for the state provided.";
                    return response;
                }
            }

            var customer = new Customer()
            {
                LGA = lga.LGAName,
                PhoneNumber = customerDto.PhoneNumber,
                StateOfResidence = state.StateName,
                Email = customerDto.Email,
                Password = PasswordHasher.Hash(customerDto.Password)
            };


            var saveResult = await this.AddAsyncReturnsId(customer);

            if (saveResult == 0)
            {
                response.HasError = true;
                response.Message = "Unable to onboard customer. Please try again later";
            }
            else
            {
                _thirdpartyIntegration.RequestOTP(customerDto.PhoneNumber);
                response.HasError = false;
                response.Message = " Check Phone number for OTP verification";
            }

            return response;
        }

        public async Task<ResponseModel> VerifiyCustomer(VerifyCustomerDto varifyCustomerDto)
        {
            var response = new ResponseModel();

            var existingCustomer = await this.GetAsync(x => x.PhoneNumber == varifyCustomerDto.PhoneNumber);
            if (existingCustomer == null)
            {
                response.HasError = true;
                response.Message = "No customer found for verification with the Phone Number provided";
            }
            else if (existingCustomer.IsVerified)
            {
                response.HasError = true;
                response.Message = "Customer email already verified";
            }
            else
            {
                var verifyOtpResponse = _thirdpartyIntegration.VerifyOTP(varifyCustomerDto.PhoneNumber, varifyCustomerDto.OTP);
                if (verifyOtpResponse)
                {
                    existingCustomer.IsVerified = true;
                    var updateResponse = await UpdateAsync(existingCustomer);
                    if (updateResponse)
                    {
                        response.HasError = false;
                        response.Message = "Customer Phone number verified successfully";
                    }
                    else
                    {
                        response.HasError = true;
                        response.Message = "Customer Phone Number could not be verified";
                    }
                }
                else
                {
                    response.HasError = true;
                    response.Message = "Invalid OTP entered";
                }
            }

            return response;
        }

       
    }
}
