using Auth.Interfaces;
using Auth.Models;
using Common.Models;
using Common.Messages;
using Common.Result;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Auth.Services
{
    public class CredentialService : ICredentialService
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IBus _bus;
        public CredentialService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IBus bus)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _bus = bus;
        }

        public async Task<Result> ChangeEmail(EmailChangeModel model, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "User not found" };

            if (!(await _userManager.CheckPasswordAsync(user, model.Password)))
                return new Result { Success = false, ResultMessage = ResultMessage.Unauthorized, Message = "Wrong credentials" };

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
            var result = await _userManager.ChangeEmailAsync(user, model.NewEmail, token);

            if (!result.Succeeded)
                return new Result { Success = false, Errors = result.Errors };

            user.UserName = model.NewEmail;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return new Result { Success = false, Errors = result.Errors };

            return new Result { Success = true, Message = "Email changed" };
        }

        public async Task<Result> ChangePassword(CredentialChangeTokenModel model, string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "User not found" };

            var result = await _userManager.ResetPasswordAsync(user, model.ChangeToken, model.NewValue);

            return new Result { Success = true, Message = "Password changed" };
        }

        public async Task<Result> SendChangeEmailToken(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            throw new NotImplementedException();
        }

        public async Task<Result> SendChangePasswordToken(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "User not found" };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string message = "Enter this code: " + token;

            _bus.PubSub.Publish(new SendEmailMessage { Message = message, To = user.Email, Topic = "Password change confirmation" });

            return new Result { Success = true, Message = "Email sent" };
        }
    }
}
