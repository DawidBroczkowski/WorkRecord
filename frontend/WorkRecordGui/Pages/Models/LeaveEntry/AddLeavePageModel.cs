using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.LeaveEntry;

namespace WorkRecordGui.Pages.Models.LeaveEntry
{
    public class AddLeavePageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private ILeaveEntryService _leaveEntryService;
        private IEmployeeService _employeeService;
        private INavigationService _navigationService;
        public ObservableCollection<GetEmployeeDto> Employees { get; set; } = new();
        public ObservableCollection<GetEmployeeDto> FilteredEmployees { get; set; } = new();
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                FilteredEmployees = new ObservableCollection<GetEmployeeDto>(Employees
                    .Where(e => (e.FirstName + " " + e.LastName).Contains(value, StringComparison.OrdinalIgnoreCase)));
                OnPropertyChanged(nameof(FilteredEmployees));
            }
        }

        public ObservableCollection<LeaveType> LeaveTypes { get; set; }
        private LeaveType _selectedLeaveType = LeaveType.sick;
        public LeaveType SelectedLeaveType
        {
            get => _selectedLeaveType;
            set
            {
                _selectedLeaveType = value;
                Leave.LeaveType = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                Leave.StartDate = new DateTime(value.Year, value.Month, value.Day, Leave.StartDate.Hour, Leave.StartDate.Minute, 0);
                OnPropertyChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                Leave.EndDate = new DateTime(value.Year, value.Month, value.Day, Leave.EndDate.Hour, Leave.EndDate.Minute, 0); ;
                OnPropertyChanged();
            }
        }

        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                Leave.StartDate = new DateTime(Leave.StartDate.Year, Leave.StartDate.Month, Leave.StartDate.Day, value.Hours, value.Minutes, 0);
                OnPropertyChanged();
            }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                Leave.EndDate = new DateTime(Leave.EndDate.Year, Leave.EndDate.Month, Leave.EndDate.Day, value.Hours, value.Minutes, 0);
                OnPropertyChanged();
            }
        }

        private CreateLeaveEntryDto _leave = new();
        public CreateLeaveEntryDto Leave
        {
            get => _leave;
            set
            {
                _leave = value;
                OnPropertyChanged();
            }
        }

        private GetEmployeeDto _selectedEmployee = new();
        public GetEmployeeDto SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                Leave.EmployeeId = value.Id;
                OnPropertyChanged();
            }
        }
        public ICommand EmployeeTappedCommand { get; }
        public ICommand AddLeaveTappedCommand { get; }

        public AddLeavePageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Employees = new ObservableCollection<GetEmployeeDto>();
            LeaveTypes = new ObservableCollection<LeaveType>(Enum.GetValues(typeof(LeaveType)).Cast<LeaveType>());
            FilteredEmployees = Employees;
            EmployeeTappedCommand = new Command<GetEmployeeDto>(OnEmployeeTapped);
            AddLeaveTappedCommand = new Command(AddLeave);
            _serviceProvider = serviceProvider;
            _leaveEntryService = _serviceProvider.GetRequiredService<ILeaveEntryService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now + TimeSpan.FromDays(1);
            StartTime = new TimeSpan(8, 0, 0);
            EndTime = new TimeSpan(16, 0, 0);
        }

        private void OnEmployeeTapped(GetEmployeeDto employee)
        {
            SelectedEmployee = employee;
        }

        private async void AddLeave()
        {
            try
            {
                await _leaveEntryService.AddLeaveEntryAsync(Leave, _cts.Token);
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task loadEmployeesAsync()
        {
            var employees = await _employeeService.GetEmployeesAsync(_cts.Token);
            Employees = new ObservableCollection<GetEmployeeDto>(employees);
            FilteredEmployees = Employees;
            OnPropertyChanged(nameof(FilteredEmployees));
        }

        public override async Task InitializeAsync()
        {
            await loadEmployeesAsync();
        }
    }
}
