using Common.Messages;
using Common.Models;
using Common.Result;
using Microsoft.AspNetCore.Identity;

namespace Personell.Interfaces
{
    public interface IPersonellService
    {
        Task<Result> AssignRole(Roles role, Guid userId);
        Task<Result<Roles>> GetRole(Guid userId);
        Task<Result<List<PersonellResponseModel>>> GetPersonell(GetPersonellRequest request);
        Task<Result<PersonellResponseModel>> GetManagerInfo(GetManagerInfoRequest request);
        Task UpdatePersonellInfo(Guid id);
    }
}
