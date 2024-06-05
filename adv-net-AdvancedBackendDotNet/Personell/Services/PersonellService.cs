using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Personell.Data;
using Personell.Data.Models;
using Personell.Interfaces;
using System.Collections.Generic;

namespace Personell.Services
{
    public class PersonellService : IPersonellService
    {
        private PersonellDbContext _dbContext;
        private IBus _bus;
        public PersonellService(PersonellDbContext dbContext, IBus bus) 
        {
            _dbContext = dbContext;
            _bus = bus;
        }

        //переделать в дженерик че я придумал вообще лол
        //ну или так пока оставить
        public async Task<Result> AssignRole(Roles role, Guid userId)
        {
            Console.WriteLine(role);
            Console.WriteLine(userId);
            Console.WriteLine("Adding");
            var personell = await _dbContext.Personell.FindAsync(userId);

            if (personell != null)
                _dbContext.Personell.Remove(personell);

            var profileResponse = _bus.Rpc.Request<GetProfileRequest, Result<ProfileResponseModel>>(new GetProfileRequest { Id = userId });

            if (!profileResponse.Success)
                return profileResponse.CastToResult();

            var profile = profileResponse.Load;

            if (role == Roles.Manager)
            {
                Manager manager = new Manager
                {
                    UserId = userId,
                    Name = profile.Name,
                    Surname = profile.Surname,
                    SecondName = profile.SecondName,
                    Email = profile.Email,
                    Roles = role,
                };
                _dbContext.Add(manager);
            }
            else if (role == Roles.MainManager)
            {
                MainManager manager = new MainManager
                {
                    UserId = userId,
                    Name = profile.Name,
                    Surname = profile.Surname,
                    SecondName = profile.SecondName,
                    Email = profile.Email,
                    Roles = role,
                };
                _dbContext.Add(manager);
            }
            else if (role == Roles.Admin)
            {
                Admin admin = new Admin
                {
                    UserId = userId,
                    Name = profile.Name,
                    Surname = profile.Surname,
                    SecondName = profile.SecondName,
                    Email = profile.Email,
                    Roles = role,
                };
                _dbContext.Add(admin);
            }
            else if (role == Roles.None)
            {
                _dbContext.SaveChanges();
                return new Result
                {
                    Success = true,
                    Message = "Role removed",
                    ResultMessage = ResultMessage.Success
                };
            }

            _dbContext.SaveChanges();

            return new Result
            {
                Success = true,
                Message = "Role assigned",
                ResultMessage = ResultMessage.Success
            };
        }

        public async Task<Result<Roles>> GetRole(Guid userId)
        {
            var user = await _dbContext.Personell.FindAsync(userId);

            if (user == null)
                return new Result<Roles> { Success = false};

            return new Result<Roles> { Load = user.Roles, Success = true };
        }

        public async Task<Result<List<PersonellResponseModel>>> GetPersonell(GetPersonellRequest request)
        {
            var result = _dbContext.Personell
                .Where(p => request.email.Length == 0 ? true : p.Email.Contains(request.email))
                .Select(p => new PersonellResponseModel
                {
                    Email = p.Email,
                    Name = p.Name,
                    Surname = p.Surname,
                    SecondName = p.SecondName,
                    Id = p.UserId,
                    Role = p.Roles,
                })
                .ToList();

            foreach (var item in result)
            {
                if (item.Role == Roles.Manager)
                {
                    var manager = _dbContext.Managers.Find(item.Id);
                    item.FacultyId = manager == null ? Guid.Empty : manager.FacultyId;
                    item.FacultyName = manager == null ? string.Empty : manager.FacultyName;
                }
            }

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));

            return new Result<List<PersonellResponseModel>> { Success = true, Load = result };
        }
        public async Task<Result<PersonellResponseModel>> GetManagerInfo(GetManagerInfoRequest request)
        {
            var personell = await _dbContext.Personell.FindAsync(request.ManagerId);
            if (personell == null)
                return new Result<PersonellResponseModel> { Success = false, ResultMessage = ResultMessage.NotFound, Message = "Manager not found" };

            var manager = _dbContext.Managers.Find(request.ManagerId);

            return new Result<PersonellResponseModel>
            {
                Success = true,
                Load = new PersonellResponseModel
                {
                    Email = personell.Email,
                    Name = personell.Name,
                    Surname = personell.Surname,
                    SecondName = personell.SecondName,
                    Id = personell.UserId,
                    Role = personell.Roles,
                    FacultyId = manager == null ? null : manager.FacultyId,
                    FacultyName = manager == null ? null : manager.FacultyName
                }
            };
        }
        public async Task UpdatePersonellInfo(Guid id)
        {
            var result = _bus.Rpc.Request<GetProfileRequest, Result<ProfileResponseModel>>(new GetProfileRequest { Id = id });

            if (!result.Success || result.Load == null)
                return;

            var personell = _dbContext.Personell.Find(id);

            if (personell == null)
                return;

            personell.Name = result.Load.Name;
            personell.Surname = result.Load.Surname;
            personell.SecondName = result.Load.SecondName;
            personell.Email = result.Load.Email;

            _dbContext.SaveChanges();
        }
    }
}
