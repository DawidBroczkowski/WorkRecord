using WorkRecord.Application;
using WorkRecord.Infrastructure.Config;

namespace WorkRecord.API
{
    public class PlanManagerBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfigManager _configManager;

        public PlanManagerBackgroundService(IServiceScopeFactory serviceScopeFactory, IConfigManager configManager)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configManager = configManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_configManager.GetPlanUpdateSeconds()), stoppingToken);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var planManager = scope.ServiceProvider.GetRequiredService<IPlanManager>();
                    DateTime currentDate = DateTime.Now;
                    DateTime nextMonth = currentDate.AddMonths(1);
                    await planManager.UpdateFutureEntriesAsync(currentDate, nextMonth, stoppingToken);
                }
            }
        }
    }
}
