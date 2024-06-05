using Auth.Models;
using Common.Result;
using Common.Models;

namespace Auth.Interfaces
{
    public interface ICredentialService
    {
        Task<Result> SendChangeEmailToken(string id);
        Task<Result> SendChangePasswordToken(string id);

        Task<Result> ChangeEmail(EmailChangeModel model, string id);
        Task<Result> ChangePassword(CredentialChangeTokenModel model, string id);
    }
}
