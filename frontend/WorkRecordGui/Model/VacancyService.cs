using System.Text;
using System.Text.Json;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Shared.Dtos.Vacancy;

namespace WorkRecordGui.Model
{
    public class VacancyService : IVacancyService
    {
        private IHttpClientFactory _clientFactory;
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public VacancyService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<GetVacancyDto>> GetVacanciesAsync(CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync("", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancies = JsonSerializer.Deserialize<List<GetVacancyDto>>(json, options);
            return vacancies!;
        }

        public async Task<GetVacancyDto?> GetVacancyAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"{id}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancy = JsonSerializer.Deserialize<GetVacancyDto>(json, options);
            return vacancy;
        }

        public async Task AddVacancyAsync(CreateVacancyDto createVacancyDto, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var json = JsonSerializer.Serialize(createVacancyDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("", data, cancellationToken);
        }

        public async Task UpdateVacancyAsync(UpdateVacancyDto updateVacancyDto, CancellationToken cancellationToken)
        {
            //var client = _clientFactory.CreateClient("Vacancy");
            //var json = JsonSerializer.Serialize(updateVacancyDto);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");
            //await client.PutAsync("", data, cancellationToken);
        }

        public async Task ChangeVacancyStatusAsync(int id, bool isActive, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.PatchAsync($"Status/{id}/{isActive}", null, cancellationToken);
        }

        public async Task DeleteVacancyAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            await client.DeleteAsync($"{id}", cancellationToken);
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"Employee/{employeeId}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancies = JsonSerializer.Deserialize<List<GetVacancyDto>>(json, options);
            return vacancies!;
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByPositionAsync(Position position, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"Position?position={position}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancies = JsonSerializer.Deserialize<List<GetVacancyDto>>(json, options);
            return vacancies!;
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByIsActiveAsync(bool isActive, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"Active/{isActive}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancies = JsonSerializer.Deserialize<List<GetVacancyDto>>(json, options);
            return vacancies!;
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByOccurrenceDayAsync(DayOfWeek occurrenceDay, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"Day/{occurrenceDay}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancies = JsonSerializer.Deserialize<List<GetVacancyDto>>(json, options);
            return vacancies!;
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByPlannedEmployeeIdAsync(int plannedEmployeeId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"PlannedEmployee/{plannedEmployeeId}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancies = JsonSerializer.Deserialize<List<GetVacancyDto>>(json, options);
            return vacancies!;
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByWeekPlanAndPositionAsync(int weekPlanId, Position position, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"WeekPlanAndPosition/{weekPlanId}/{position}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancies = JsonSerializer.Deserialize<List<GetVacancyDto>>(json, options);
            return vacancies!;
        }

        public async Task<List<GetVacancyDto>> GetVacanciesByWeekPlanIdAsync(int weekPlanId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"WeekPlan/{weekPlanId}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var vacancies = JsonSerializer.Deserialize<List<GetVacancyDto>>(json, options);
            return vacancies!;
        }

        public async Task<bool> VacancyExistsAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Vacancy");
            var response = await client.GetAsync($"Exists/{id}", cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
