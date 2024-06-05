using Common.Messages;
using Common.Models;
using Common.Result;
using Personell.Interfaces;

namespace Personell.Handlers
{
    public class PersonellMessageHandler
    {
        private IPersonellService _personellService;
        private IAppointmentService _appointmentService;
        public PersonellMessageHandler(IPersonellService personellService, IAppointmentService appointmentService)
        {
            _personellService = personellService;
            _appointmentService = appointmentService;
        }

        public async Task HandleAssignUserMessage(AssignRoleMessage msg)
        {
            try
            {
                var result = await _personellService.AssignRole(msg.Role, msg.UserId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task HandleAppointToFacultyMessage(AppointToFacultyMessage msg)
        {
            
            try
            {
                await _appointmentService.AppointManagerToFaculty(msg.ManagerId, Guid.Parse(msg.FacultyId));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task HandleUpdatePersonell(UpdatePersonell msg)
        {
            
            try
            {
                await _personellService.UpdatePersonellInfo(msg.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<Result<Roles>> HandleGetRoleRequest(GetRoleRequest request) => await _personellService.GetRole(request.UserId);
        public async Task<Result<PersonellResponseModel>> HandleAppointToApplicationRequest(AppointManagerRequest msg) => await _appointmentService.AppointManagerToApplication(msg);
        public async Task<Result<List<PersonellResponseModel>>> HandleGetPersonell(GetPersonellRequest request) => await _personellService.GetPersonell(request);
        public async Task<Result<PersonellResponseModel>> HandleGetManager(GetManagerInfoRequest request) => await _personellService.GetManagerInfo(request);

        public static PersonellMessageHandler GetHandler(WebApplication app) => app.Services.CreateScope().ServiceProvider.GetRequiredService<PersonellMessageHandler>();
    }
}
