using Common.Messages;
using Common.Models;
using Common.Result;
using Dictionary.Data;
using Dictionary.Data.Models;
using Dictionary.HostedServices;
using Dictionary.Interfaces;
using Dictionary.Models;
using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

//для начала просто сделаю загрузку, потом придумаю что-нибудь поумнее
//по идее нужно просто искать новые и удаленные: первые добавлять, вторые помечать соответствующе
namespace Dictionary.Services
{
    public class DictionaryUpdateService : IDictionaryUpdateService
    {
        private DictionaryDbContext _dbContext;
        private HttpClient _httpClient;
        //private IHostedService _programUpdateService;
        private IConfiguration _configuration;
        private IBus _bus;

        //private static bool _isStarted { get;  set; }
        public DictionaryUpdateService(DictionaryDbContext dbContext, HttpClient httpClient, IConfiguration configuration, IBus bus)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
            _configuration = configuration;
            _bus = bus;
            /*
            string username = "student";
            string password = "ny6gQnyn4ecbBrP9l1Fz";
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            */
        }

        public async Task UpdateDocumentTypes(bool isSecondTime = false)
        {
            try
            {
                var isStarted = await GetIsUpdating();
                if (isStarted.IsStarted && !isSecondTime)
                    return;

                Guid updateId = Guid.NewGuid();

                await StartUpdate(updateId);

                var result = await _httpClient.GetAsync(_configuration.GetSection("ExternalService:Host").Value + _configuration.GetSection("ExternalService:DocumentTypes").Value);
                var jsonString = await result.Content.ReadAsStringAsync();
                List<DocumentType> items = JsonConvert.DeserializeObject<List<DocumentType>>(jsonString);
                items = items.Select(item =>
                {
                    item.EducationLevelId = item.EducationLevel.Id;
                    item.EducationLevel = _dbContext.EducationLevels.Find(item.EducationLevelId);
                    item.NextEducationLevels = _dbContext.EducationLevels
                        .Where(level => item.NextEducationLevels
                        .Select(nextLevel => nextLevel.Id)
                        .Contains(level.Id)).ToList();

                    return item;
                }).ToList();

                List<DocumentType> oldItems = _dbContext.DocumentTypes.Include(i => i.NextEducationLevels).ToList();

                _dbContext.DocumentTypes.RemoveRange(oldItems);
                _dbContext.DocumentTypes.AddRange(items);

                Console.WriteLine("update ended");

                _dbContext.SaveChanges();

                if (!isSecondTime)
                    await UpdateDocumentTypes(true);

                await FinishUpdate(updateId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task UpdateEducationLevels()
        {
            try
            {
                var isStarted = await GetIsUpdating();
                if (isStarted.IsStarted)
                    return;

                Guid updateId = Guid.NewGuid();

                await StartUpdate(updateId);

                var result = await _httpClient.GetAsync(_configuration.GetSection("ExternalService:Host").Value + _configuration.GetSection("ExternalService:EducationLevels").Value);
                var jsonString = await result.Content.ReadAsStringAsync();
                List<EducationLevel> items = JsonConvert.DeserializeObject<List<EducationLevel>>(jsonString);

                EducationLevel idZeroItem = items.FirstOrDefault(item => item.Id == 0);

                items = items.Where(item => item.Id != 0).ToList();

                List<EducationLevel> oldItems = _dbContext.EducationLevels.ToList();

                _dbContext.EducationLevels.RemoveRange(oldItems);
                _dbContext.EducationLevels.AddRange(items);

                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(items));

                foreach (var item in items)
                    Console.WriteLine(item.Id.ToString() + " " + item.Name);

                if (idZeroItem != null)
                {
                    _dbContext.Entry(idZeroItem).State = EntityState.Added;
                }

                Console.WriteLine("update ended");

                _dbContext.SaveChanges();

                await FinishUpdate(updateId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task UpdateFaculties()
        {
            try
            {
                var isStarted = await GetIsUpdating();
                if (isStarted.IsStarted)
                    return;

                Guid updateId = Guid.NewGuid();

                await StartUpdate(updateId);

                var result = await _httpClient.GetAsync(_configuration.GetSection("ExternalService:Host").Value + _configuration.GetSection("ExternalService:Faculties").Value);
                var jsonString = await result.Content.ReadAsStringAsync();
                List<Faculty> items = JsonConvert.DeserializeObject<List<Faculty>>(jsonString);

                List<Faculty> oldItems = _dbContext.Faculties.ToList();

                _dbContext.Faculties.RemoveRange(oldItems);
                _dbContext.Faculties.AddRange(items);

                _dbContext.SaveChanges();

                await FinishUpdate(updateId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public async Task UpdatePrograms()
        {
            var isStarted = await GetIsUpdating();
            if (isStarted.IsStarted)
                return;

            Guid updateId = Guid.NewGuid();

            await StartUpdate(updateId);

            try
            {
                //var update = new UpdateState { Id = Guid.NewGuid(), IsFinished = false };
                //_dbContext.UpdateStates.Add(update);
                //_dbContext.SaveChanges();

                List<Guid> oldIds = _dbContext.Programs.Select(program => program.Id).ToList();

                List<EducationProgram> oldPrograms = _dbContext.Programs.ToList();
                _dbContext.Programs.RemoveRange(oldPrograms);

                var result = await _httpClient.GetAsync(_configuration.GetSection("ExternalService:Host").Value + _configuration.GetSection("ExternalService:Programs").Value + "?page=1&size=10");
                var jsonString = await result.Content.ReadAsStringAsync();

                //List<EducationProgram> items = JsonConvert.DeserializeObject<List<EducationProgram>>(jsonString);
                //_dbContext.Programs.AddRange(items);

                List<Guid> newIds = new List<Guid>();

                GetProgramsResponse response = JsonConvert.DeserializeObject<GetProgramsResponse>(jsonString);

                for (int i = 1; i < response.Pagination.Count; i++)
                {
                    result = await _httpClient.GetAsync(_configuration.GetSection("ExternalService:Host").Value + _configuration.GetSection("ExternalService:Programs").Value + "?page=" + i.ToString() + "&size=10");
                    jsonString = await result.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<GetProgramsResponse>(jsonString);

                    var items = response.Programs;

                    items = items.Select(item =>
                    {
                        item.EducationLevel = _dbContext.EducationLevels.Find(item.EducationLevel.Id);
                        item.Faculty = _dbContext.Faculties.Find(item.Faculty.Id);

                        return item;
                    }).ToList();

                    newIds.AddRange(items.Select(item => item.Id));

                    _dbContext.Programs.AddRange(response.Programs);
                }

                var saveTask = _dbContext.SaveChangesAsync();

                var deletedIds = oldIds.Except(newIds).ToList();

                await _bus.PubSub.PublishAsync(new DeleteProgramsMessage { Ids = deletedIds});

                await saveTask;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            await FinishUpdate(updateId);
        }

        public async Task<GetUpdateResponse> GetIsUpdating()
        {
            var isStarted = _dbContext.UpdateStates.Any(update => !update.IsFinished);
            return new GetUpdateResponse { IsStarted = isStarted };
        }

        private async Task StartUpdate(Guid id)
        {
            var update = new UpdateState { Id = id, IsFinished = false, UpdateTime = DateTime.UtcNow };
            await _dbContext.AddAsync(update);
            await _dbContext.SaveChangesAsync();
        }

        private async Task FinishUpdate(Guid id)
        {
            var update = await _dbContext.UpdateStates.FindAsync(id);

            if (update == null)
                return;

            update.IsFinished = true;
            await _dbContext.SaveChangesAsync();
        }

    }
}
