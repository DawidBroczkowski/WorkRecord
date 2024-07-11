using System.Text;
using System.Text.Json;
using WorkRecordGui.Shared.Dtos.User;

namespace WorkRecordGui.Model
{
    public class UserService : IUserService
    {
        private IHttpClientFactory _clientFactory;
        private Session _session;

        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public UserService(IHttpClientFactory clientFactory, Session session)
        {
            _clientFactory = clientFactory;
            _session = session;
        }

        public async Task<string> LoginAsync(string login, string password)
        {
            var client = _clientFactory.CreateClient("JWT");
            LoginUserDto loginUserDto = new LoginUserDto
            {
                Login = login,
                Password = password
            };
            var json = JsonSerializer.Serialize(loginUserDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("", data);
            var token = await response.Content.ReadAsStringAsync();
            _session.Token = token;
            var user = await GetUserAsync(_session.UserId);
            _session.User = user!;
            return token;
        }

        public async Task<List<GetUserDto>> GetUsersAsync()
        {
            var client = _clientFactory.CreateClient("User");
            var response = await client.GetAsync("");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<List<GetUserDto>>(json, options);
            return user!;
        }

        public async Task<GetUserDto?> GetUserAsync(int id)
        {
            var client = _clientFactory.CreateClient("User");
            var response = await client.GetAsync($"{id}");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<GetUserDto>(json, options);
            return user;
        }

        public async Task RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var client = _clientFactory.CreateClient("User");
            var json = JsonSerializer.Serialize(registerUserDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("register", data);
        }
    }
}
