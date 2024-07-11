using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WorkRecordGui.Model;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.User;

namespace WorkRecordGui.Pages.Models
{
    public class LoginPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IUserService _userService;
        private INavigationService _navigationService;
        public ICommand LoginCommand { get; }
        private string _login = string.Empty;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public LoginPageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userService = _serviceProvider.GetRequiredService<IUserService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            LoginCommand = new Command(async () => await LoginAsync());
            Login = "admin";
            Password = "admin123";
        }

        public async Task LoginAsync()
        {
            try
            {
                await _userService.LoginAsync(_login, _password);
                if (Session.User.EmployeeId is not null)
                {
                    await _navigationService.NavigateToAsync(typeof(EmployeePageModel), Session.User.EmployeeId!.Value);
                }
            }
            catch (Exception e)
            {
                try
                {
                    // temp solution
                    var users = await _userService.GetUsersAsync();
                    int? id = users.FirstOrDefault(u => u.Login == _login)?.Id;
                    if (id is not null)
                    {
                        RegisterUserDto registerUserDto= new RegisterUserDto
                        {
                            Id = id.Value,
                            Login = _login,
                            Password = _password
                        };
                        await _userService.RegisterUserAsync(registerUserDto);
                        await _userService.LoginAsync(_login, _password);
                        if (Session.User.EmployeeId is not null)
                        {
                            await _navigationService.NavigateToAsync(typeof(EmployeePageModel), Session.User.EmployeeId!.Value);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Login failed");
                }
            }
            
        }

        public override async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }
    }
}
