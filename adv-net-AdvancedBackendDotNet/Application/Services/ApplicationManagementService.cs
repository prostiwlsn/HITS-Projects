using Application.Data;
using Application.Interfaces;
using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Services
{
    public class ApplicationManagementService : IApplicationManagementService
    {
        private ApplicationDbContext _dbContext;
        private IBus _bus;
        public ApplicationManagementService(ApplicationDbContext context, IBus bus)
        {
            _dbContext = context;
            _bus = bus;
        }
        public async Task<Result> EditApplicationStatus(Guid id, ApplicationStatus status)
        {
            var application = await _dbContext.Applications.FindAsync(id);

            if (application == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound };

            if (status == ApplicationStatus.Any)
                return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "Wrong status" };
            else if (status == ApplicationStatus.Closed)
                application.IsClosed = true;
            else
            {
                application.Status = status;
                application.IsClosed = false;
            }

            application.LastChange = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            await _bus.PubSub.PublishAsync(new SendEmailMessage { To = application.Email, Message = "Your application's status has been updated", Topic = "noreply"});

            return new Result { Success = true, ResultMessage = ResultMessage.Success };
        }

        public async Task<Result> AppointManager(AppointManagerMessage msg)
        {
            var application = await _dbContext.Applications.FindAsync(msg.UserId);

            if (application == null)
                return new Result { Success = false, Message = "Application not found" };

            application.LastChange = DateTime.UtcNow;
            application.ManagerId = Guid.Parse(msg.ManagerId);

            var managerInfoResult = await _bus.Rpc.RequestAsync<AppointManagerRequest, Result<PersonellResponseModel>>(new AppointManagerRequest { ManagerId = Guid.Parse(msg.ManagerId), UserId = msg.UserId});
            if (!managerInfoResult.Success || managerInfoResult.Load == null)
                return managerInfoResult.CastToResult();

            application.ManagerName = managerInfoResult.Load.Name + " " + managerInfoResult.Load.Surname + " " + managerInfoResult.Load.SecondName;
            application.ManagerEmail = managerInfoResult.Load.Email;

            _bus.PubSub.Publish(new SendEmailMessage { To = application.Email, Message = "A manager has been appointed to your application: " + managerInfoResult.Load.Name + " " + managerInfoResult.Load.Surname + " " + managerInfoResult.Load.SecondName, Topic = "noreply" });

            await _dbContext.SaveChangesAsync();

            return new Result { Success = true };
        }

        public async Task<Result<List<ApplicationInfoModel>>> GetApplications(uint size, uint page, string name, List<Guid> faculties, ApplicationStatus status, bool? isManagerAppointed, Guid managerId, bool? isDescending)
        {
            var query = _dbContext.Applications.Include(a => a.ChosenPrograms).AsQueryable();

            if (name.Length != 0)
            {
                query = query.Where(q => (q.Name + " " + q.Surname + " " + q.SecondName).Contains(name));
            }

            if (faculties.Count != 0)
            {
                query = query.Where(q => q.ChosenPrograms
                    .Select(program => program.FacultyId)
                    .Any(facultyId => faculties.Contains(facultyId)));
            }

            if (status != ApplicationStatus.Any)
            {
                if (status == ApplicationStatus.Closed)
                {
                    query = query.Where(q => q.IsClosed);
                }
                else
                {
                    query = query.Where(q => q.Status == status);
                }
            }

            if (isManagerAppointed != null)
            {
                if (isManagerAppointed == true)
                {
                    query = query.Where(q => q.ManagerId == managerId);
                }
                else
                {
                    query = query.Where(q => q.ManagerId == null);
                }
            }

            if (isDescending != null)
            {
                if (isDescending == true)
                {
                    query = query.OrderByDescending(q => q.LastChange);
                }
                else
                {
                    query = query.OrderBy(q => q.LastChange);
                }
            }

            if (page == 0)
                page = 1;

            var load = query.Skip((int)(size*(page - 1))).Take((int)size)
                .Select(q => new ApplicationInfoModel
            {
                UserId = q.UserId,
                Name = q.Name,
                Surname = q.Surname,
                SecondName = q.SecondName,
                Email = q.Email,
                Status = q.Status,
                IsClosed = q.IsClosed,
                EducationDocumentId = q.EducationDocumentId,
                EducationDocumentName = q.EducationDocumentName,
                NextEducationLevels = q.NextEducationLevels,
                ManagerId = q.ManagerId,
                ManagerName = q.ManagerName,
                LastChange = q.LastChange,
            }).ToList();

            return new Result<List<ApplicationInfoModel>> { Success = true, Load = load };
        }

        public async Task<Result<ApplicationInfoModel>> GetApplication(Guid userId)
        {
            var application = await _dbContext.Applications.Include(a => a.ChosenPrograms).FirstOrDefaultAsync(a => userId == a.UserId);

            if (application == null)
                return new Result<ApplicationInfoModel> { Success = false, ResultMessage = ResultMessage.NotFound, Message = "ApplicationNotFound" };

            return new Result<ApplicationInfoModel> { Success = true, Load = new ApplicationInfoModel 
            {
                UserId = application.UserId,
                Name = application.Name,
                Surname = application.Surname,
                SecondName = application.SecondName,
                Email = application.Email,
                Status = application.Status,
                IsClosed = application.IsClosed,
                EducationDocumentId = application.EducationDocumentId,
                EducationDocumentName = application.EducationDocumentName,
                NextEducationLevels = application.NextEducationLevels,
                ManagerId = application.ManagerId,
                ManagerName = application.ManagerName,
                LastChange = application.LastChange,
                Faculties = application.ChosenPrograms.Select(program => program.FacultyId).ToList(),
            } };

        }
    }
}
