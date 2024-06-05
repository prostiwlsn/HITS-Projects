using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Personell.Data;
using Personell.Interfaces;

namespace Personell.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IBus _bus;
        private readonly PersonellDbContext _dbContext;
        public AppointmentService(IBus bus, PersonellDbContext dbContext)
        {
            _bus = bus;
            _dbContext = dbContext;
        }
        public async Task AppointManagerToFaculty(Guid managerId, Guid facultyId)
        {
            Console.WriteLine("xddddd");
            Console.WriteLine(managerId);
            Console.WriteLine(facultyId);
            var facultyInfo = await _bus.Rpc.RequestAsync<GetFacultyInfoRequest, Result<FacultyInfoModel>>(new GetFacultyInfoRequest { Id = facultyId});

            if (!facultyInfo.Success || facultyInfo.Load == null)
                return;

            var manager = await _dbContext.Managers.FindAsync(managerId);
            if (manager == null)
                return;

            manager.FacultyName = facultyInfo.Load.Name;
            manager.FacultyId = facultyId;  

            await _dbContext.SaveChangesAsync();

            return;
        }
        public async Task<Result<PersonellResponseModel>> AppointManagerToApplication(AppointManagerRequest msg)
        {
            var manager = await _dbContext.Managers.FindAsync(msg.ManagerId);

            Console.WriteLine("manager search xdfdddddddddddddddddddddddddddddddddddd");

            if (manager == null)
                return new Result<PersonellResponseModel> { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Manager not found" };

            Console.WriteLine("manager found");

            await _dbContext.Applications.AddAsync(new Data.Models.Application { ApplicationId = msg.UserId, Manager = manager });

            var saveTask = _dbContext.SaveChangesAsync();

            _bus.PubSub.Publish(new SendEmailMessage { To = manager.Email, Message = "You have been appointed to application", Topic = "noreply"});

            await saveTask;

            return new Result<PersonellResponseModel>
            {
                Success = true,
                Load = new PersonellResponseModel
                {
                    Email = manager.Email,
                    Name = manager.Name,
                    Surname = manager.Surname,
                    SecondName = manager.SecondName,
                    Id = manager.UserId,
                    Role = manager.Roles,
                }
            };
        }
    }
}
