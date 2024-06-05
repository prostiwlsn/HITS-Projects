using Application.Data;
using Application.Data.Models;
using Application.Interfaces;
using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.InteropServices;

namespace Application.Services
{
    public class ProgramService : IProgramService
    {
        private ApplicationDbContext _dbContext;
        private IBus _bus;
        private IApplicationService _applicationService;
        private IConfiguration _configuration;
        public ProgramService(ApplicationDbContext context, IBus bus, IApplicationService applicationService, IConfiguration configuration)
        {
            _dbContext = context;
            _bus = bus;
            _applicationService = applicationService;
            _configuration = configuration;
        }
        public async Task<Result> AddProgram(Guid userId, Guid programId)
        {
            var application = _dbContext.Applications.Find(userId);

            if (application == null)
            {
                await _applicationService.CreateApplication(userId);
                _dbContext.SaveChanges();
                application = _dbContext.Applications.Find(userId);

                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(application.NextEducationLevels));
            }

            var programInfo = _bus.Rpc.Request<GetProgramInfoRequest, Result<ProgramInfoModel>>(new GetProgramInfoRequest { Id = programId});

            if (!programInfo.Success)
                return programInfo.CastToResult();

            var programs = _dbContext.ChosenProgram.Where(program => program.ApplicationInfoId == userId).ToList();

            var maxPrograms = await GetMaxPrograms();

            if (programs.Count >= maxPrograms)
            {
                return new Result { Success = false, ResultMessage = ResultMessage.TooMany, Message = "You have chosen too many programs"};
            }
            else if (programs.Where(p => p.ProgramId == programId).Count() > 0)
            {
                return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "You have chosen the same program twice" };
            }
            else if (!application.NextEducationLevels.Contains(programInfo.Load.EducationLevelId))
            {
                Console.WriteLine(programInfo.Load.EducationLevelId);
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(application.NextEducationLevels));
                return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "You have chosen a program with unmatching education level" };
            }

            //добавить удаление програм после изменения id уровня обучения
            //добавить имя факультета

            var faculty = _bus.Rpc.Request<GetFacultyInfoRequest, Result<FacultyInfoModel>>(new GetFacultyInfoRequest { Id = programInfo.Load.FacultyId });

            var program = new ChosenProgram
            {
                ApplicationInfoId = userId,
                Application = application,
                ProgramId = programId,
                ProgramName = programInfo.Load.Name,
                FacultyId = programInfo.Load.FacultyId,
                FacultyName = faculty.Load.Name,
                Priority = (uint)(programs.Count + 1)
            };

            _dbContext.ChosenProgram.Add(program);

            application.LastChange = DateTime.UtcNow;
            _dbContext.SaveChanges();

            return new Result { Success = true };
        }
        //добавить максимум программ в appsettings
        public async Task<Result> EditProgram(Guid userId, Guid programId, uint priority)
        {
            var application = await _dbContext.Applications
                .Include(a => a.ChosenPrograms)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (application == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Application not found" };

            var program = application.ChosenPrograms.FirstOrDefault(p => p.ProgramId == programId);
            if (program == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Program not found" };

            var programs = application.ChosenPrograms.OrderBy(p => p.Priority).ToList();

            var maxPrograms = await GetMaxPrograms();

            var currentPriority = program.Priority;

            if (priority > currentPriority)
            {
                for (uint i = currentPriority; i < priority; i++)
                {
                    programs.First(p => p.Priority == i + 1).Priority = i;
                }
            }
            else if (priority < currentPriority)
            {
                for (uint i = currentPriority; i > priority; i--)
                {
                    programs.First(p => p.Priority == i - 1).Priority = i;
                }
            }
            else
            {
                return new Result { Success = true };
            }

            program.Priority = priority > maxPrograms ? maxPrograms : priority;
            application.LastChange = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return new Result { Success = true };
        }

        public async Task<Result<List<ChosenProgramInfoModel>>> GetPrograms(Guid id)
        {
            var user = _dbContext.Applications.Include(a => a.ChosenPrograms).FirstOrDefault(a => a.UserId == id);

            if (user == null)
                return new Result<List<ChosenProgramInfoModel>> { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Application not found"};

            var programs = user.ChosenPrograms
                .Select(program => new ChosenProgramInfoModel
                {
                    FacultyId = program.FacultyId,
                    FacultyName = program.FacultyName,
                    ProgramId = program.ProgramId,
                    ProgramName = program.ProgramName,
                    Priority = program.Priority
                }).ToList();

            return new Result<List<ChosenProgramInfoModel>> { Success = true, Load = programs };
        }

        public async Task<Result> RemoveProgram(Guid userId, Guid programId)
        {
            var application = _dbContext.Applications.Include(a => a.ChosenPrograms).FirstOrDefault(a => a.UserId == userId);

            if (application == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Application not found" };

            var program = application.ChosenPrograms.First(p => p.ProgramId == programId);

            if (program == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Program not found" };

            var programs = application.ChosenPrograms.OrderBy(p => p.Priority).ToList();

            var maxPrograms = await GetMaxPrograms();

            for (uint i = program.Priority; i <= programs.Count; i++)
            {
                programs[(int)i - 1].Priority = i - 1;
            }

            _dbContext.ChosenProgram.Remove(program);
            _dbContext.SaveChanges();

            application.LastChange = DateTime.UtcNow;

            return new Result { Success = true };
        }

        public async Task<uint> GetMaxPrograms() => _configuration.GetSection("MaximumPrograms").Get<uint>();

        public async Task<Result> EditPrograms(Guid userId, List<ProgramPriorityModel> programs)
        {
            Console.WriteLine("xdddddd");
            var application = await _dbContext.Applications.Include(a => a.ChosenPrograms).FirstOrDefaultAsync(a => a.UserId == userId);

            if (application == null)
                return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Application not found" };

            if (programs.Count > await GetMaxPrograms())
                return new Result { Success = false, ResultMessage = ResultMessage.Fail, Message = "Incorrect priorities" };

            programs = programs.OrderBy(program => program.Priority).ToList();

            for (uint i = 1; i <= programs.Count; i++)
            {
                var foundProgram = application.ChosenPrograms.FirstOrDefault(p => p.ProgramId == programs[(int)i - 1].Program);

                if (foundProgram == null)
                    return new Result { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Program not found" };

                foundProgram.Priority = i;
                Console.WriteLine(foundProgram.Priority.ToString() + " " + foundProgram.ProgramName);
            }

            await _dbContext.SaveChangesAsync();

            return new Result { Success = true };
        }

        public async Task HandleUpdate(List<Guid> ids)
        {
            var programs = _dbContext.ChosenProgram.Include(program => program.Application).Where(program => ids.Contains(program.ProgramId));

            foreach (var program in programs)
            {
                _bus.PubSub.Publish(new SendEmailMessage { To = program.Application.Email, Message = "A program has been deleted: " + program.ProgramName, Topic = "noreply" });
            }

            _dbContext.RemoveRange(programs);

            await _dbContext.SaveChangesAsync();
        }
    }
}
