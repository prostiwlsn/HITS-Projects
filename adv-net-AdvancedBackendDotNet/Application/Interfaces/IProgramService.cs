using Common.Models;
using Common.Result;

namespace Application.Interfaces
{
    public interface IProgramService
    {
        Task<Result<List<ChosenProgramInfoModel>>> GetPrograms(Guid id);

        Task<Result> AddProgram(Guid userId, Guid programId);
        Task<Result> RemoveProgram(Guid userId, Guid programId);
        Task<Result> EditProgram(Guid userId, Guid programId, uint priority);

        Task<Result> EditPrograms(Guid userId, List<ProgramPriorityModel> programs);
        Task<uint> GetMaxPrograms();
        Task HandleUpdate(List<Guid> ids);
    }
}
