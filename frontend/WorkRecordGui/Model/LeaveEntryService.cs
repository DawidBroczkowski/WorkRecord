using System.Text;
using System.Text.Json;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Shared.Dtos.LeaveEntry;

namespace WorkRecordGui.Model
{
    public class LeaveEntryService : ILeaveEntryService
    {
        private IHttpClientFactory _clientFactory;
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public LeaveEntryService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesAsync(CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("LeaveEntry");
            var response = await client.GetAsync("", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var leaveEntries = JsonSerializer.Deserialize<List<GetLeaveEntryDto>>(json, options);
            return leaveEntries!;
        }

        public async Task<GetLeaveEntryDto?> GetLeaveEntryAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("LeaveEntry");
            var response = await client.GetAsync($"{id}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var leaveEntry = JsonSerializer.Deserialize<GetLeaveEntryDto>(json, options);
            return leaveEntry;
        }

        public async Task AddLeaveEntryAsync(CreateLeaveEntryDto createLeaveEntryDto, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("LeaveEntry");
            var json = JsonSerializer.Serialize(createLeaveEntryDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("", data, cancellationToken);
        }

        public async Task UpdateLeaveEntryAsync(UpdateLeaveEntryDto updateLeaveEntryDto, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("LeaveEntry");
            var json = JsonSerializer.Serialize(updateLeaveEntryDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync("", data, cancellationToken);
        }

        public async Task DeleteLeaveEntryAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("LeaveEntry");
            await client.DeleteAsync($"{id}", cancellationToken);
        }

        public async Task<List<GetLeaveEntryDto>> GetLeaveEntriesByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("LeaveEntry");
            var response = await client.GetAsync($"User/{employeeId}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var leaveEntries = JsonSerializer.Deserialize<List<GetLeaveEntryDto>>(json, options);
            return leaveEntries!;
        }
    }
}
