using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WemaBankTask.Entities.DTO;

namespace WemaBankTask.Common.Integration
{
    public class ThirdPartyIntegration : IThirdPatyIntegration
    {
        private readonly IConfiguration _configuration;
        public ThirdPartyIntegration(IConfiguration configuration)
        {
            _configuration = configuration;
            var lblOneTimePassword = GenerateOTP();
        }
        public async Task<GetBankListResponseModel> GetBankList()
        {
            GetBankListResponseModel bankList;
            var actionUrl = $"{_configuration["WemaAPIBaseUrl"]}/api/Shared/GetAllBanks";
            try
            {
                var httpClientHandler = new System.Net.Http.HttpClientHandler() 
                { Proxy = new WebProxy() { BypassProxyOnLocal = true } };
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13 | SecurityProtocolType.SystemDefault;
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;
                httpClientHandler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
                var httpClient = new HttpClient(httpClientHandler, false);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var subscriptionKey = _configuration["WemaAPISubscriptionKey"];
                httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{subscriptionKey}");

                var request = await httpClient.GetAsync(actionUrl).ConfigureAwait(false);
                var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (request.StatusCode == HttpStatusCode.OK)
                {
                    bankList = JsonConvert.DeserializeObject<GetBankListResponseModel>(response);
                }
                else
                {
                    bankList = new GetBankListResponseModel()
                    {
                        HasError = true,
                        ErrorMessage = $"Unable to get a successful response from Client API. Statuscode: {request.StatusCode} - {response}"
                    };
                }
            }
            catch (Exception ex)
            {
                bankList = new GetBankListResponseModel
                {
                    HasError = true,
                    ErrorMessage = $"Statuscode: 500 - {ex.Message} :: {ex.InnerException} :: {ex.StackTrace}"
                };
            }
            return bankList;
        }

        public bool RequestOTP(string phoneNumber)
        {
          

            return true;
        }

        public bool VerifyOTP(string PhoneNumber, int oTP)
        {
            //var lblOneTimePassword = GenerateOTP();
            if (oTP == 1234567)
                return true;
            return false;
        }

        private string GenerateOTP()
        {
            int MaxSize = 4;
            //int MinSize = 0;
            char[] chars = new char[62];
            string Character;
            // a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Character = "1234567890";
            chars = Character.ToCharArray();
            int Size = MaxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            Size = MaxSize;
            data = new byte[Size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(Size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            

            return result.ToString();

        }
    }
}
