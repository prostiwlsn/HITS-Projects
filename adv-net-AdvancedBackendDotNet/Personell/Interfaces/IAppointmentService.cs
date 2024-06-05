using Common.Messages;
using Common.Models;
using Common.Result;

namespace Personell.Interfaces
{
    public interface IAppointmentService
    {
        Task AppointManagerToFaculty(Guid managerId, Guid facultyId);
        Task<Result<PersonellResponseModel>> AppointManagerToApplication(AppointManagerRequest msg);
    }
}
