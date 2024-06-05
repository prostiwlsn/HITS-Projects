using Auth.Interfaces;
using Common.Messages;
using Common.Models;
using Common.Result;

namespace Auth.Handlers
{
    public class AuthMessageHandler
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthMessageHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<UserIdResponseModel>> HandleGetUserId(GetUserIdRequest request)
        {
            Guid? id = await _authenticationService.GetUserId(request.Email);

            if (id == null)
                return new Result<UserIdResponseModel>
                {
                    Success = false,
                };

            return new Result<UserIdResponseModel>
            {
                Load = new UserIdResponseModel
                {
                    UserId = id ?? new Guid()
                },
                Success = true,
            };
        }
        
        public static AuthMessageHandler GetRpcHandler(WebApplication app) => app.Services.CreateScope().ServiceProvider.GetRequiredService<AuthMessageHandler>();
    }
}
