using Application.Data;
using Application.Data.Models;
using Application.Interfaces;
using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private ApplicationDbContext _dbContext;
        private IBus _bus;
        public ApplicationService(ApplicationDbContext context, IBus bus)
        {
            _dbContext = context;
            _bus = bus;
        }
        public async Task<Result> CreateApplication(Guid id)
        {
            var result = _bus.Rpc.Request<GetProfileRequest, Result<ProfileResponseModel>>(new GetProfileRequest { Id = id });

            if (!result.Success)
                return result.CastToResult();

            var documentResult = _bus.Rpc.Request<GetEducationDocumentInfoRequest, Result<EducationDocumentInfoModel>>(new GetEducationDocumentInfoRequest { UserId = id});

            if (!documentResult.Success)
                return result.CastToResult();

            ApplicationInfo application = new ApplicationInfo
            {
                Name = result.Load.Name,
                Surname = result.Load.Surname,
                SecondName = result.Load.SecondName,
                UserId = id,
                Email = result.Load.Email,
                Status = ApplicationStatus.Created,
                EducationDocumentName = documentResult.Load.Name,
                EducationDocumentId = documentResult.Load.DocumentTypeId,
                NextEducationLevels = documentResult.Load.NextEducationLevels,
            };

            application.LastChange = DateTime.UtcNow;

            _dbContext.Applications.Add(application);
            _dbContext.SaveChanges();

            return new Result { Success = true };
        }

        public async Task<Result> DeleteApplication(Guid id)
        {
            var application = _dbContext.Applications.Find(id);

            if (application == null)
                return new Result { ResultMessage = ResultMessage.NotFound, Success = false };

            _dbContext.Applications.Remove(application);
            _dbContext.SaveChanges();

            return new Result { Success = true };
        }

        public async Task<Result> EditApplication(Guid id)
        {
            var result = _bus.Rpc.Request<GetProfileRequest, Result<ProfileResponseModel>>(new GetProfileRequest { Id = id });

            if (!result.Success)
                return result.CastToResult();

            var application = _dbContext.Applications.Find(id);

            if (application == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound };

            application.Name = result.Load.Name;
            application.Name = result.Load.Name;
            application.Surname = result.Load.Surname;
            application.SecondName = result.Load.SecondName;
            application.UserId = id;
            application.Email = result.Load.Email;

            return new Result { Success = true };
        }

        public async Task<Result> UpdateApplication(Guid id, bool isEditedByApplicant = false)
        {
            var result = _bus.Rpc.Request<GetProfileRequest, Result<ProfileResponseModel>>(new GetProfileRequest { Id = id });

            if (!result.Success)
                return result.CastToResult();

            var application = _dbContext.Applications.Find(id);

            if (application == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound };

            //добавить обновление данных о документе об образовании
            var educationDocumentInfo = _bus.Rpc.Request<GetEducationDocumentInfoRequest, Result<EducationDocumentInfoModel>>(new GetEducationDocumentInfoRequest { UserId = id });

            application.Name = result.Load.Name;
            application.Name = result.Load.Name;
            application.Surname = result.Load.Surname;
            application.SecondName = result.Load.SecondName;
            application.UserId = id;
            application.Email = result.Load.Email;

            if (!educationDocumentInfo.Success)
                return result.CastToResult();

            if (educationDocumentInfo.Load.DocumentTypeId != application.EducationDocumentId)
            {
                var programs = application.ChosenPrograms.ToList();
                _dbContext.ChosenProgram.RemoveRange(programs);
            }

            application.LastChange = DateTime.UtcNow;

            application.EducationDocumentName = educationDocumentInfo.Load.Name;
            application.EducationDocumentId = educationDocumentInfo.Load.DocumentTypeId;
            application.NextEducationLevels = educationDocumentInfo.Load.NextEducationLevels;

            if(isEditedByApplicant && application.Status == ApplicationStatus.Denied)
                application.Status = ApplicationStatus.Pending;

            _dbContext.SaveChanges();

            return new Result { Success = true };
        }

        public async Task<bool> GetApplicationStatus(Guid id)
        {
            var application = await _dbContext.Applications.FindAsync(id);
            if (application == null)
                return false;

            if (application.IsClosed)
                return true;
            else return false;
        }
    }
}
