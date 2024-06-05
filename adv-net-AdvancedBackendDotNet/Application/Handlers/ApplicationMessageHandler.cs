using Common.Messages;
using Common.Models;
using Common.Result;
using Common;
using Application.Interfaces;
using Application.Services;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace Application.Handlers
{
    public class ApplicationMessageHandler
    {
        private readonly IApplicationService _applicationService;
        private readonly IApplicationManagementService _applicationManagementService;
        private readonly IProgramService _programService;
        public ApplicationMessageHandler(IApplicationService applicationService, IApplicationManagementService applicationManagementService, IProgramService programService) 
        {
            _applicationService = applicationService;
            _applicationManagementService = applicationManagementService;
            _programService = programService;
        }

        public async Task<bool> HandleGetStatus(GetApplicationStatusRequest request)
        {
            return await _applicationService.GetApplicationStatus(request.Id);
        }

        public async Task HandleDeletePrograms(DeleteProgramsMessage msg)
        {
            try
            {
                await _programService.HandleUpdate(msg.Ids);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task HandleEditPrograms(EditProgramsMessage msg)
        {
            
            try
            {
                await _programService.EditPrograms(msg.UserId, msg.Programs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task HandleEditProgram(EditProgramMessage msg)
        {
            
            try
            {
                await _programService.EditProgram(msg.UserId, msg.ProgramId, msg.Priority);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task HandleDeleteProgram(DeleteProgramMessage msg)
        {
            
            try
            {
                await _programService.RemoveProgram(msg.UserId, msg.ProgramId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task<Result<List<ChosenProgramInfoModel>>> HandleGetPrograms(GetProgramsMessage msg) => await _programService.GetPrograms(msg.UserId);

        public async Task HandleApplicationUpdateMessage(UpdateApplicationMessage msg)
        {
            
            try
            {
                await _applicationService.UpdateApplication(msg.Id, msg.IsEditedByApplicant);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async Task HandleEditApplicationStatusMessage(EditApplicationStatusMessage msg)
        {
            
            try
            {
                await _applicationManagementService.EditApplicationStatus(msg.UserId, msg.Status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task HandleAppointManager(AppointManagerMessage msg)
        {
            
            try
            {
                await _applicationManagementService.AppointManager(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<Result<List<ApplicationInfoModel>>> HadleGetApplicationsRequest(GetApplicationsRequest request) =>
            await _applicationManagementService.GetApplications(request.size, request.page, request.name, request.faculties.Select(faculty => Guid.Parse(faculty)).ToList(), 
                request.status, request.isManagerAppointed, request.managerId, request.isDescending);
        public async Task<Result<ApplicationInfoModel>> HandleGetApplicationRequest(GetApplicationRequest request) =>
            await _applicationManagementService.GetApplication(request.UserId);

        public static ApplicationMessageHandler GetHandler(WebApplication app) => app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationMessageHandler>();
    }
}
