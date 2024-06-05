using Auth.Data;
using Quartz;

namespace Auth.Jobs
{
    public class SessionCleanupJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public SessionCleanupJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

                var now = DateTime.UtcNow;
                var expiredSessions = dbContext.Sessions.Where(s => s.Expires < now);

                dbContext.Sessions.RemoveRange(expiredSessions);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
