using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Profile.Data;
using Profile.Data.Models;
using Profile.Interfaces;
//using Profile.Models;

namespace Profile.Services
{
    public class ProfileService : IProfileService
    {
        private ProfileDbContext _dbContext;
        private readonly IBus _bus;
        public ProfileService(ProfileDbContext dbContext, IBus bus) 
        { 
            _dbContext = dbContext;
            _bus = bus;
        }
        public async Task<Result> Register(RegistrationFormRequest request)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(request));
            var profile = new ProfileInformation
            {
                UserId = request.UserId,
                Name = request.Name,
                Surname = request.Surname,
                SecondName = request.Secondname,
                Gender = request.Gender,
                Citizenship = request.Citizenship,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Passport = new Passport
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                },
                EducationDocument = new EducationDocument
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId
                }
            };

            try
            {
                await _dbContext.Profiles.AddAsync(profile);

                await _dbContext.SaveChangesAsync();

                return new Result { ResultMessage = ResultMessage.Success, Success = true, Message = "Ok" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return new Result { ResultMessage = ResultMessage.Fail, Success = false, Message = "Something went wrong" };
            }
        }

        public async Task<Result<ProfileResponseModel>> GetProfile(Guid id)
        {
            var profile = await _dbContext.Profiles.FindAsync(id);

            if (profile == null)
                return new Result<ProfileResponseModel> { Success = false, ResultMessage = ResultMessage.NotFound };

            return new Result<ProfileResponseModel> 
            { 
                ResultMessage = ResultMessage.Success,
                Success = true,
                Load = new ProfileResponseModel()
                {
                    Name = profile.Name,
                    Surname = profile.Surname,
                    SecondName = profile.SecondName,
                    Gender = profile.Gender,
                    Citizenship = profile.Citizenship,
                    Email = profile.Email,
                    PhoneNumber = profile.PhoneNumber
                }
            };
        }

        public async Task<Result> EditProfile(ProfileEditModel model, Guid id, bool isEditedByUser = false)
        {
            var profile = _dbContext.Profiles.Find(id);

            if (profile == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Profile not found" };

            profile.Name = model.Name;
            profile.Surname = model.Surname;
            profile.SecondName = model.SecondName;
            profile.Gender = model.Gender;
            profile.Citizenship = model.Citizenship;
            profile.PhoneNumber = model.PhoneNumber;

            try
            {
                await _dbContext.SaveChangesAsync();
                _bus.PubSub.Publish(new UpdateApplicationMessage { Id = id, IsEditedByApplicant = isEditedByUser });
                _bus.PubSub.Publish(new UpdatePersonell { Id = id });
            }
            catch (Exception ex)
            {
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = ex.ToString() };
            }

            return new Result { Message = "OK" };
        }
    }
}
