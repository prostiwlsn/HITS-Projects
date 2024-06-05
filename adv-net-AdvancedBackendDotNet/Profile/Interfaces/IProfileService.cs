using Common.Messages;
using Common.Models;
using Common.Result;
//using Profile.Models;

namespace Profile.Interfaces
{
    public interface IProfileService
    {
        Task<Result> Register(RegistrationFormRequest request);
        Task<Result<ProfileResponseModel>> GetProfile(Guid id);
        Task<Result> EditProfile(ProfileEditModel model, Guid id, bool isEditedByUser = false);
    }
}
