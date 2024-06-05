
using Dictionary.Data;
using Dictionary.Data.Models;
using Dictionary.Models;
using EasyNetQ.Management.Client.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;

namespace Dictionary.HostedServices
{
    public class ProgramUpdateService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private bool _isStarted = false;
        public ProgramUpdateService(IServiceScopeFactory serviceScopeFactory, DictionaryDbContext dbContext, HttpClient httpClient, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_isStarted) return Task.CompletedTask;

            _isStarted = true;
            UpdatePrograms();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task UpdatePrograms()
        {
            /*
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<DictionaryDbContext>();
                if (_dbContext.UpdateStates.Where(update => !update.IsFinished).ToList().Count != 0)
                    return;

                var update = new UpdateState { Id = Guid.NewGuid(), IsFinished = false };
                _dbContext.UpdateStates.Add(update);
                _dbContext.SaveChanges();

                List<EducationProgram> oldPrograms = _dbContext.Programs.ToList();
                _dbContext.Programs.RemoveRange(oldPrograms);

                var result = await _httpClient.GetAsync(_configuration.GetSection("ExternalService:Host").Value + _configuration.GetSection("ExternalService:Programs").Value + "?page=1&size=10");
                var jsonString = await result.Content.ReadAsStringAsync();

                //List<EducationProgram> items = JsonConvert.DeserializeObject<List<EducationProgram>>(jsonString);
                //_dbContext.Programs.AddRange(items);

                GetProgramsResponse response = JsonConvert.DeserializeObject<GetProgramsResponse>(jsonString);

                for (int i = 1; i < response.Pagination.Count; i++)
                {
                    result = await _httpClient.GetAsync(_configuration.GetSection("ExternalService:Host").Value + _configuration.GetSection("ExternalService:Programs").Value + "?page=" + i.ToString() + "&size=10");
                    jsonString = await result.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<GetProgramsResponse>(jsonString);

                    _dbContext.Programs.AddRange(response.Programs);
                }

                _dbContext.SaveChanges();

                _isStarted = false;
            }
            */
        }
    }
}
