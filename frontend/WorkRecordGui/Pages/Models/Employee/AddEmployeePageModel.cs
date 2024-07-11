using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Employee;

namespace WorkRecordGui.Pages.Models.Employee
{
    public class AddEmployeePageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IEmployeeService _employeeService;
        private INavigationService _navigationService;
        public ObservableCollection<Position> Positions { get; set; }
        private Position _selectedPosition = Position.dentist;

        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                Employee.Position = value;
                OnPropertyChanged();
            }
        }

        private CreateEmployeeDto _employee = new();
        public CreateEmployeeDto Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddEmployeeCommand { get; }

        public AddEmployeePageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Positions = new ObservableCollection<Position>(Enum.GetValues(typeof(Position)).Cast<Position>());
            _serviceProvider = serviceProvider;
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            AddEmployeeCommand = new Command(AddEmployee);
        }

        public override async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        private async void AddEmployee()
        {
            try
            {
                await _employeeService.AddEmployeeAsync(Employee, _cts.Token);
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
