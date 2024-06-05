using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using MVCAdmin.Interfaces;
using MVCAdmin.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MVCAdmin.Services
{
    public class PersonellRequestService : IPersonellRequestService
    {
        private IBus _bus;
        private IAuthRequestService _authRequestService;
        private IConfiguration _configuration;
        public PersonellRequestService(IBus bus, IAuthRequestService authRequestService, IConfiguration configuration)
        {
            _bus = bus;
            _authRequestService = authRequestService;
            _configuration = configuration;
        }

        public async Task AssignRole(AssignRoleModel model)
        {
            var id = await _bus.Rpc.RequestAsync<GetUserIdRequest, Result<UserIdResponseModel>>(new GetUserIdRequest { Email = model.Email });

            if (id == null || !id.Success)
                return;

            await _bus.PubSub.PublishAsync(new AssignRoleMessage { UserId = id.Load.UserId, Role = model.Role});
        }

        public async Task CreateAdminAccount()
        {
            UserRegistrationModel model = new UserRegistrationModel
            {
                Email = _configuration.GetSection("AdminAccount:Email").Value,
                Password = _configuration.GetSection("AdminAccount:Password").Value,
                Name = _configuration.GetSection("AdminAccount:Name").Value,
                Surname = _configuration.GetSection("AdminAccount:Surname").Value,
                Secondname = _configuration.GetSection("AdminAccount:Secondname").Value,
                PhoneNumber = _configuration.GetSection("AdminAccount:PhoneNumber").Value,
                Gender = (Gender)int.Parse(_configuration.GetSection("AdminAccount:Gender").Value),
                Citizenship = _configuration.GetSection("AdminAccount:Citizenship").Value
            };

            var result = await _authRequestService.Register(model);

            if (result == null || !result.Success)
                return;

            var id = await _bus.Rpc.RequestAsync<GetUserIdRequest, Result<UserIdResponseModel>>(new GetUserIdRequest { Email = model.Email});

            if (!id.Success) 
                return;

            _bus.PubSub.Publish(
                new AssignRoleMessage
                {
                    UserId = id.Load.UserId,
                    Role = Roles.Admin,
                });
        }

        public async Task<string> GetMyRole(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenObject = handler.ReadJwtToken(token);
            return tokenObject.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;
        }
        public async Task<string> GetMyId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenObject = handler.ReadJwtToken(token);
            return tokenObject.Claims.FirstOrDefault(claim => claim.Type == "id").Value;
        }
        public async Task<Result<List<PersonellResponseModel>>> GetPersonell(GetPersonellRequest request)
        {
            var result = _bus.Rpc.Request<GetPersonellRequest, Result<List<PersonellResponseModel>>>(request);
            return result;
        }

        public async Task AppointToFaculty(AppointToFacultyMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);
        }

        public async Task AppointManager(AppointManagerMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);
        }

        public async Task<Result<PersonellResponseModel>> GetManagerInfo(GetManagerInfoRequest request)
        {
            var result = await _bus.Rpc.RequestAsync<GetManagerInfoRequest, Result<PersonellResponseModel>>(request);
            return result;
        }
    }
}
