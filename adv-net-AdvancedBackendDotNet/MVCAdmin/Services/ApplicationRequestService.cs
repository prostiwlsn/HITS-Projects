using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using MVCAdmin.Interfaces;
using System.Collections.Generic;

namespace MVCAdmin.Services
{
    public class ApplicationRequestService : IApplicationRequestService
    {
        private readonly IBus _bus;
        private readonly IProfileRequestService _profileRequestService;
        private readonly IPersonellRequestService _personellRequestService;
        public ApplicationRequestService(IBus bus, IProfileRequestService profileRequestService, IPersonellRequestService personellRequestService) 
        {
            _bus = bus;
            _profileRequestService = profileRequestService;
            _personellRequestService = personellRequestService;
        }

        public async Task EditApplication(EditApplicationModel msg)
        {
            var response = await GetApplication(new GetApplicationRequest { UserId = msg.AppointManagerMessage.UserId });

            if (!response.Success || response.Load == null)
                return;
            if (response.Load.Status != msg.EditApplicationStatusMessage.Status)
                await EditApplicationStatus(msg.EditApplicationStatusMessage);

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(msg.AppointManagerMessage));

            if (msg.AppointManagerMessage.ManagerId != null &&  response.Load.ManagerId != Guid.Parse(msg.AppointManagerMessage.ManagerId))
                await _personellRequestService.AppointManager(msg.AppointManagerMessage);

            await _profileRequestService.EditApplicantProfile(msg.ProfileEditMessage);
        }

        public async Task EditApplicationStatus(EditApplicationStatusMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);
        }

        public async Task<Result<ApplicationInfoModel>> GetApplication(GetApplicationRequest request)
        {
            var response = await _bus.Rpc.RequestAsync<GetApplicationRequest, Result<ApplicationInfoModel>>(request);
            return response;
        }

        public async Task<Result<List<ApplicationInfoModel>>> GetApplications(GetApplicationsRequest request)
        {
            var response = await _bus.Rpc.RequestAsync<GetApplicationsRequest, Result<List<ApplicationInfoModel>>>(request)??new Result<List<ApplicationInfoModel>> { Success = false, ResultMessage = ResultMessage.ServerFail};

            return response;
        }

        public async Task<Result<List<ChosenProgramInfoModel>>> GetPrograms(GetProgramsMessage msg)
        {
            var response = await _bus.Rpc.RequestAsync<GetProgramsMessage, Result<List<ChosenProgramInfoModel>>>(msg) ?? new Result<List<ChosenProgramInfoModel>> { Success = false, ResultMessage = ResultMessage.ServerFail };

            return response;
        }

        public async Task<Result> EditPrograms(EditProgramsMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);

            return new Result { Success = true };
        }
        public async Task<Result> EditProgram(EditProgramMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);

            return new Result { Success = true };
        }
        public async Task DeleteProgram(DeleteProgramMessage msg)
        {
            await _bus.PubSub.PublishAsync(msg);
        }
    }
}
