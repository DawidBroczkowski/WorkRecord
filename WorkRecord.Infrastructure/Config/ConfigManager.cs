using System.Text.Json;

namespace WorkRecord.Infrastructure.Config
{
    public class ConfigManager : IConfigManager
    {
        private Config _config;
        private readonly string _fileName;

        public ConfigManager(string fileName)
        {
            _fileName = fileName;
            _config = new Config();
            LoadConfigAsync().Wait();
        }

        private async Task LoadConfigAsync()
        {
            if (File.Exists(_fileName) is false)
            {
                _config.WeekPlanId = 1;
                await SaveConfigAsync();
                return;
            }
            string jsonString = await File.ReadAllTextAsync(_fileName);
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                Config config = new Config();
                await SaveConfigAsync();
                return;
            }
            _config = JsonSerializer.Deserialize<Config>(jsonString)!;
        }

        private async Task SaveConfigAsync()
        {
            await File.WriteAllTextAsync(_fileName, JsonSerializer.Serialize(_config));
        }

        public int GetWeekPlanId()
        {
            return _config.WeekPlanId;
        }

        public void SetWeekPlanId(int id)
        {
            _config.WeekPlanId = id;
            SaveConfigAsync().Wait();
        }

        public int GetPlanUpdateSeconds()
        {
            return _config.PlanUpdateSeconds;
        }

        public void SetPlanUpdateSeconds(int seconds)
        {
            _config.PlanUpdateSeconds = seconds;
            SaveConfigAsync().Wait();
        }
    }
}
