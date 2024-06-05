using Auth.Data;
using Auth.Models;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;

namespace Auth.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<TokenResponseModel>> Login(string email, string password);
        Task<Result<TokenResponseModel>> Refresh(Guid sessionId);
        Task<Result> Logout();
        Task<Result> Register(UserRegistrationModel model);
        Task<Guid?> GetUserId(string email);
    }
}
