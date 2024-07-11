using System.Text;
using System.Text.Json;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Shared.Dtos.ChartEntry;

namespace WorkRecordGui.Model
{
    public class ChartEntryService : IChartEntryService
    {
        private IHttpClientFactory _clientFactory;
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ChartEntryService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesAsync(CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            var response = await client.GetAsync("", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var chartEntries = JsonSerializer.Deserialize<List<GetChartEntryDto>>(json, options);
            return chartEntries!;
        }

        public async Task<GetChartEntryDto?> GetChartEntryAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            var response = await client.GetAsync($"{id}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var chartEntry = JsonSerializer.Deserialize<GetChartEntryDto>(json, options);
            return chartEntry;
        }

        public async Task AddChartEntryAsync(CreateChartEntryDto createChartEntryDto, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            var json = JsonSerializer.Serialize(createChartEntryDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("", data, cancellationToken);
            Console.WriteLine(response.Content);
        }

        public async Task UpdateChartEntryAsync(UpdateChartEntryDto updateChartEntryDto, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            var json = JsonSerializer.Serialize(updateChartEntryDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync("", data, cancellationToken);
        }

        public async Task DeleteChartEntryAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            await client.DeleteAsync($"{id}", cancellationToken);
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            var response = await client.GetAsync($"employee/{employeeId}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var chartEntries = JsonSerializer.Deserialize<List<GetChartEntryDto>>(json, options);
            return chartEntries!;
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime startDate, DateTime endDate, int employeeId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            var response = await client.GetAsync($"DateRange/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}/{employeeId}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var chartEntries = JsonSerializer.Deserialize<List<GetChartEntryDto>>(json, options);
            return chartEntries!;
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateRangeAndPositionAsync(DateTime startDate, DateTime endDate, Position position, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            var response = await client.GetAsync($"Position/DateRange?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&position={position}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var chartEntries = JsonSerializer.Deserialize<List<GetChartEntryDto>>(json, options);
            return chartEntries!;
        }

        public async Task<List<GetChartEntryDto>> GetChartEntriesByDateOverlapAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("ChartEntry");
            var response = await client.GetAsync($"DateRange/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var chartEntries = JsonSerializer.Deserialize<List<GetChartEntryDto>>(json, options);
            return chartEntries!;
        }
    }
}
