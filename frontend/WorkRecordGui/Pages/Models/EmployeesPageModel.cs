using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Employee;

namespace WorkRecordGui.Pages.Models
{
    public class EmployeesPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private INavigationService _navigationService;
        private IEmployeeService _employeeService;
        public ObservableCollection<GetEmployeeDto> Employees { get; set; } = new();
        public ObservableCollection<GetEmployeeDto> FilteredEmployees { get; set; } = new();
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                filterSearch();
            }
        }
        public ObservableCollection<Position> Positions { get; set; }
        private Position _selectedPosition = Position.dentist;
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged();
            }
        }

        private bool _isFilterActive;
        public bool IsFilterActive
        {
            get => _isFilterActive;
            set
            {
                _isFilterActive = value;
                filterSearch();
                OnPropertyChanged();
            }
        }

        public ICommand EmployeeTappedCommand { get; }
        public ICommand AddEmployeeCommand { get; }

        public EmployeesPageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Positions = new ObservableCollection<Position>(Enum.GetValues(typeof(Position)).Cast<Position>());
            _serviceProvider = serviceProvider;
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            EmployeeTappedCommand = new Command<int>(onNavigateToEmployee);
            AddEmployeeCommand = new Command(() => _navigationService.NavigateToAsync(typeof(AddEmployeePageModel)));
        }

        private async void onNavigateToEmployee(int id)
        {
            await _navigationService.NavigateToAsync(typeof(EmployeePageModel), id);
        }

        private async Task loadDataAsync()
        {
            try
            {
                var employees = await _employeeService.GetEmployeesAsync(_cts.Token);
                Employees = new ObservableCollection<GetEmployeeDto>(employees);
                FilteredEmployees = Employees;
                OnPropertyChanged(nameof(FilteredEmployees));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override async Task InitializeAsync()
        {
            await loadDataAsync();
            OnPropertyChanged();
        }

        private void filterSearch()
        {
            List<GetEmployeeDto> filtered;
            filtered = Employees
               .Where(e => string.IsNullOrEmpty(SearchText) || (e.FirstName + " " + e.LastName).Contains(SearchText, StringComparison.OrdinalIgnoreCase))
               .Where(e => !IsFilterActive || e.Position == SelectedPosition)
               .ToList();
            FilteredEmployees = new ObservableCollection<GetEmployeeDto>(filtered);
            OnPropertyChanged(nameof(FilteredEmployees));
        }
    }
}
