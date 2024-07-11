using System.Text;
using System.Text.Json;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Shared.Dtos.ChartEntry;

namespace WorkRecordGui.Model
{
    class PlanManagerService : IPlanManagerService
    {
        private IHttpClientFactory _clientFactory;
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public PlanManagerService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task ChangeCurrentPlanAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("PlanManager");
            await client.PutAsync($"{id}", null, cancellationToken);
        }

        public async Task<int> GetCurrentPlanIdAsync()
        {
            var client = _clientFactory.CreateClient("PlanManager");
            var response = await client.GetAsync("");
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(json, options);
        }

        public async Task UpdateFutureEntriesAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("PlanManager");
            var json = JsonSerializer.Serialize(new { From = from, To = to });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("", data, cancellationToken);
        }

        public async Task<List<GetUnfilledChartEntryDto>> GetUnfilledVacanciesAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("PlanManager");
            var response = await client.GetAsync($"Unfilled/{from:o}/{to:o}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var entries = JsonSerializer.Deserialize<List<GetUnfilledChartEntryDto>>(json, options);
            return entries!;
        }
    }
}
