using System.Text;
using System.Text.Json;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Shared.Dtos.Employee;

namespace WorkRecordGui.Model
{
    public class EmployeeService : IEmployeeService
    {
        private IHttpClientFactory _clientFactory;
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        public EmployeeService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<GetEmployeeDto>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            var response = await client.GetAsync("", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var employees = JsonSerializer.Deserialize<List<GetEmployeeDto>>(json, options);
            return employees!;
        }

        public async Task<GetEmployeeDto?> GetEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            var response = await client.GetAsync($"{id}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            GetEmployeeDto employee = JsonSerializer.Deserialize<GetEmployeeDto>(json, options)!;
            return employee;
        }

        public async Task AddEmployeeAsync(CreateEmployeeDto createEmployeeDto, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            var json = JsonSerializer.Serialize(createEmployeeDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("", data, cancellationToken);
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            var json = JsonSerializer.Serialize(updateEmployeeDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync("", data, cancellationToken);
        }

        public async Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            await client.DeleteAsync($"{id}", cancellationToken);
        }

        public async Task<List<GetEmployeeDto>> GetEmployeesByPositionAsync(Position position, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            var response = await client.GetAsync($"Position/{position}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var employees = JsonSerializer.Deserialize<List<GetEmployeeDto>>(json, options);
            return employees!;
        }

        public async Task AddChildAsync(int employeeId, DateTime birthday, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            var formattedDate = birthday.ToString("yyyy-MM-dd"); // Adjust the format if necessary
            await client.PostAsync($"AddChild?employeeId={employeeId}&birthday={formattedDate}", null, cancellationToken);
        }

        public async Task RemoveChildAsync(int employeeId, ushort index, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            await client.DeleteAsync($"RemoveChild/{employeeId}/{index}", cancellationToken);
        }

        public async Task<List<DateTime>> GetChildrenBirthdaysAsync(int employeeId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            var response = await client.GetAsync($"ChildrenBirthdays/{employeeId}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var birthdays = JsonSerializer.Deserialize<List<DateTime>>(json, options);
            return birthdays!;
        }

        public async Task<GetLeaveDaysDto> GetLeaveDaysAsync(int employeeId, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            var response = await client.GetAsync($"LeaveDays/{employeeId}", cancellationToken);
            var json = await response.Content.ReadAsStringAsync();
            var leaveDays = JsonSerializer.Deserialize<GetLeaveDaysDto>(json, options);
            return leaveDays!;
        }

        public async Task NewYearResetAsync(CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("Employee");
            await client.PostAsync("NewYearReset", null, cancellationToken);
        }
    }
}
