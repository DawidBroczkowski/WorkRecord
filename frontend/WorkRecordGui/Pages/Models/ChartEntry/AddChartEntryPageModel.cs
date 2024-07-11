using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.ChartEntry;
using WorkRecordGui.Shared.Dtos.Employee;

namespace WorkRecordGui.Pages.Models.ChartEntry
{
    public class AddChartEntryPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IChartEntryService _chartEntryService;
        private INavigationService _navigationService;
        private IEmployeeService _employeeService;
        private IVacancyService _vacancyService;

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                filterEmployees();
                OnPropertyChanged();
            }
        }

        private CreateChartEntryDto _chartEntry = new CreateChartEntryDto();
        public CreateChartEntryDto ChartEntry
        {
            get => _chartEntry;
            set
            {
                _chartEntry = value;
                OnPropertyChanged();
            }
        }

        private Position _selectedPosition;
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                if (_selectedPosition == value)
                {
                    return;
                }
                _selectedPosition = value;
                filterEmployees();
                OnPropertyChanged();
            }
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                ChartEntry.StartDate = new DateTime(value.Year, value.Month, value.Day, ChartEntry.StartDate.Hour, ChartEntry.StartDate.Minute, 0);
                OnPropertyChanged();
            }
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                ChartEntry.EndDate = new DateTime(value.Year, value.Month, value.Day, ChartEntry.EndDate.Hour, ChartEntry.EndDate.Minute, 0);
                OnPropertyChanged();
            }
        }

        private TimeSpan _startTime = TimeSpan.FromHours(8);
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                ChartEntry.StartDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, value.Hours, value.Minutes, 0);
                OnPropertyChanged();
            }
        }

        private TimeSpan _endTime = TimeSpan.FromHours(16);
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                ChartEntry.EndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, value.Hours, value.Minutes, 0);
                OnPropertyChanged();
            }
        }

        private GetEmployeeDto _selectedEmployee;
        public GetEmployeeDto SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                ChartEntry.EmployeeId = value.Id;
                Task.Run(loadEmployeeFutureChartEntriesAsync);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Position> Positions { get; set; }
        public List<GetEmployeeDto> Employees { get; set; } = new();
        public ObservableCollection<GetEmployeeDto> FilteredEmployees { get; set; } = new();
        public ObservableCollection<GetChartEntryDto> ChartEntries { get; set; } = new();

        public ICommand AddChartEntryCommand { get; }
        public ICommand EmployeeSelectedCommand { get; }

        public AddChartEntryPageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _chartEntryService = _serviceProvider.GetRequiredService<IChartEntryService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();
            Positions = new ObservableCollection<Position>(Enum.GetValues<Position>());
            AddChartEntryCommand = new Command(async () => await addChartEntry());
            EmployeeSelectedCommand = new Command<GetEmployeeDto>(onEmployeeSelected);
        }

        public AddChartEntryPageModel(GetUnfilledChartEntryDto chartEntry, Position position, IServiceProvider serviceProvider) : this(serviceProvider)
        {
            ChartEntry = new CreateChartEntryDto
            {
                VacancyId = chartEntry.VacancyId
            };
            StartDate = chartEntry.StartDate;
            EndDate = chartEntry.EndDate;
            StartTime = chartEntry.StartDate.TimeOfDay;
            EndTime = chartEntry.EndDate.TimeOfDay;
            SelectedPosition = position;
            OnPropertyChanged(nameof(ChartEntry));
            OnPropertyChanged(nameof(SelectedPosition));
        }

        private async Task addChartEntry()
        {
            await _chartEntryService.AddChartEntryAsync(ChartEntry, _cts.Token);
            await _navigationService.GoBackAsync();
        }

        private async Task loadEmployeesAsync()
        {
            Employees = await _employeeService.GetEmployeesAsync(_cts.Token);
        }

        private async Task loadEmployeeFutureChartEntriesAsync()
        {
            var chartEntries = await _chartEntryService.GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime.Now, DateTime.MaxValue, SelectedEmployee.Id, _cts.Token);
            ChartEntries = new ObservableCollection<GetChartEntryDto>(chartEntries);
            OnPropertyChanged(nameof(ChartEntries));
        }

        private void filterEmployees()
        {
            var filtered = Employees
                .Where(e => e.Position == SelectedPosition)
                .Where(e => string.IsNullOrEmpty(SearchText) || (e.FirstName + " " + e.LastName).Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredEmployees = new ObservableCollection<GetEmployeeDto>(filtered);
            OnPropertyChanged(nameof(FilteredEmployees));
        }

        private void onEmployeeSelected(GetEmployeeDto employee)
        {
            SelectedEmployee = employee;
        }

        public override async Task InitializeAsync()
        {
            await loadEmployeesAsync();
            filterEmployees();
        }
    }
}
