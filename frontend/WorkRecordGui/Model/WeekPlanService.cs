using System.Text;
using System.Text.Json;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Shared.Dtos.WeekPlan;

namespace WorkRecordGui.Model
{
    public class WeekPlanService : IWeekPlanService
    {
        private IHttpClientFactory _clientFactory;
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public WeekPlanService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<GetWeekPlanDto>> GetWeekPlansAsync(CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("WeekPlan");
            var response = await client.GetAsync("", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var weekPlans = JsonSerializer.Deserialize<List<GetWeekPlanDto>>(json, options);
            return weekPlans!;
        }

        public async Task<GetWeekPlanDto?> GetWeekPlanAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("WeekPlan");
            var response = await client.GetAsync($"{id}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var weekPlan = JsonSerializer.Deserialize<GetWeekPlanDto>(json, options);
            return weekPlan;
        }

        public async Task AddWeekPlanAsync(string name, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("WeekPlan");
            await client.PostAsync($"{name}", null, cancellationToken);
        }

        public async Task UpdateWeekPlanAsync(UpdateWeekPlanDto updateWeekPlanDto, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("WeekPlan");
            var json = JsonSerializer.Serialize(updateWeekPlanDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync("", data, cancellationToken);
        }

        public async Task DeleteWeekPlanAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("WeekPlan");
            await client.DeleteAsync($"{id}", cancellationToken);
        }
    }
}
