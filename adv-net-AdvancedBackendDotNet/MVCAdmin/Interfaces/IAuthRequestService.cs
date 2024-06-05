using Common.Models;
using Common.Result;

namespace MVCAdmin.Interfaces
{
    public interface IAuthRequestService
    {
        Task<Result<TokenResponseModel>> Login(LoginModel model);
        Task<Result> Register(UserRegistrationModel model);
        Task<Result<TokenResponseModel>> Refresh(string refreshToken);
        Task<Result> SendPasswordChangeToken(string token);
        Task<Result> ChangePassword(CredentialChangeTokenModel model, string token);
        Task<Result> ChangeEmail(EmailChangeModel model, string token);
    }
}
