using Common.Models;
using Common.Result;
using MVCAdmin.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace MVCAdmin.Services
{
    public class AuthRequestService : IAuthRequestService
    {
        private HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AuthRequestService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Result> Register(UserRegistrationModel model)
        {
            using (JsonContent content = JsonContent.Create(model))
            {
                //_httpClient.PostAsync();
                var result = await _httpClient
                    .PostAsync(_configuration.GetSection("URls:Auth:Host").Value + _configuration.GetSection("URls:Auth:Register").Value, content);
                if (result.IsSuccessStatusCode)
                    return new Result
                    {
                        ResultMessage = ResultMessage.Success,
                        Success = true,
                        Message = "Ok"
                    };
                else
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    //JsonConvert.DeserializeObject<Result>(responseContent);
                    return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "Something went wrong"}; 
                }
            }
        }

        public async Task<Result<TokenResponseModel>> Login(LoginModel model)
        {
            using (JsonContent content = JsonContent.Create(model))
            {
                var response = await _httpClient.PostAsync(_configuration.GetSection("URls:Auth:Host").Value + _configuration.GetSection("URls:Auth:Login").Value, content);
                if (!response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ResultErrors errors = JsonConvert.DeserializeObject<ResultErrors>(jsonString);
                    return new Result<TokenResponseModel> { ResultMessage = ResultMessage.Fail, Success = false };
                }
                else
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    TokenResponseModel tokenResponseModel = JsonConvert.DeserializeObject<TokenResponseModel>(jsonString);
                    return new Result<TokenResponseModel> { Success = true, Message = "Ok", Load = tokenResponseModel };
                }
            }
        }

        public async Task<Result<TokenResponseModel>> Refresh(string refreshToken)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _configuration.GetSection("URls:Auth:Host").Value + _configuration.GetSection("URls:Auth:Refresh").Value))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshToken);

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    TokenResponseModel load = JsonConvert.DeserializeObject<TokenResponseModel>(jsonString);
                    return new Result<TokenResponseModel> { Success = true, Message = "Ok", Load = load };
                }
                else
                {
                    return new Result<TokenResponseModel> { Success = false };
                }
            }
        }

        public async Task<Result> SendPasswordChangeToken(string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, _configuration.GetSection("URls:Auth:Host").Value + _configuration.GetSection("URls:Auth:Password").Value))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new Result { Success = true, Message = "Ok" };
                }
                else
                {
                    return new Result { Success = false };
                }
            }
        }
        public async Task<Result> ChangePassword(CredentialChangeTokenModel model, string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, _configuration.GetSection("URls:Auth:Host").Value + _configuration.GetSection("URls:Auth:Password").Value))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (JsonContent content = JsonContent.Create(model))
                {
                    request.Content = content;
                    HttpResponseMessage response = await _httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        return new Result { Success = true, Message = "Ok" };
                    }
                    else
                    {
                        return new Result { Success = false };
                    }
                }
            }
        }

        public async Task<Result> ChangeEmail(EmailChangeModel model, string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, _configuration.GetSection("URls:Auth:Host").Value + _configuration.GetSection("URls:Auth:Email").Value))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (JsonContent content = JsonContent.Create(model))
                {
                    request.Content = content;
                    HttpResponseMessage response = await _httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        return new Result { Success = true, Message = "Ok" };
                    }
                    else
                    {
                        return new Result { Success = false };
                    }
                }
            }
        }
    }
}
