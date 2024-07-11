using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkRecordGui.Model;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.ChartEntry;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.ChartEntry;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.LeaveEntry;

namespace WorkRecordGui.Pages.Models
{
    public class ChartEntriesPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IChartEntryService _chartEntryService;
        private INavigationService _navigationService;
        private IEmployeeService _employeeService;

        private List<GetChartEntryDto> _chartEntries;
        private List<GetEmployeeDto> _employees;
        public List<ChartEntryWithEmployee> ChartEntriesWithEmployees { get; set; } = new();
        public ObservableCollection<ChartEntryWithEmployee> FilteredChartEntriesWithEmployees { get; set; } = new();
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                Task.Run(async () => await filterEntriesAsync());
            }
        }
        public ObservableCollection<Position> Positions { get; set; } = new();

        private Position _selectedPosition;
        public Position SelectedPosition
        {
            get 
            { 
                return _selectedPosition; 
            } 
            set 
            {
                _selectedPosition = value;
                Task.Run(async () => await filterEntriesAsync());
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private bool _enableDateRange = false;
        public bool EnableDateRange
        {
            get => _enableDateRange;
            set
            {
                _enableDateRange = value;
                OnPropertyChanged(nameof(EnableDateRange));
            }
        }

        private bool _isFilterActive = false;
        public bool IsFilterActive
        {
            get => _isFilterActive;
            set
            {
                _isFilterActive = value;
                Task.Run(async () => await filterEntriesAsync());
                OnPropertyChanged();
            }
        }

        public ICommand EntryTappedCommand => new Command<int>(async (id) =>
        {
            await _navigationService.NavigateToAsync(typeof(ChartEntryPageModel), id);
        });

        public ICommand AddEntryCommand => new Command(async () =>
        {
            await _navigationService.NavigateToAsync(typeof(AddChartEntryPageModel));
        });

        public ICommand GetEntriesCommand => new Command(async () =>
        {
            await loadChartEntriesAsync();
        });

        public ChartEntriesPageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _chartEntryService = _serviceProvider.GetRequiredService<IChartEntryService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();

            Positions = new ObservableCollection<Position>(Enum.GetValues(typeof(Position)).Cast<Position>());
        }

        private async Task loadEmployeesAsync()
        {
            _employees = await _employeeService.GetEmployeesAsync(_cts.Token);
        }

        private async Task loadChartEntriesAsync()
        {
            if (EnableDateRange is true)
            {
                _chartEntries = await _chartEntryService.GetChartEntriesByDateOverlapAsync(StartDate!.Value, EndDate!.Value, _cts.Token);
            }
            else
            {
                _chartEntries = await _chartEntryService.GetChartEntriesAsync(_cts.Token);
            }

            ChartEntriesWithEmployees.Clear();
            foreach (var chartEntry in _chartEntries)
            {
                var employee = _employees.First(e => e.Id == chartEntry.EmployeeId);
                ChartEntriesWithEmployees.Add(new ChartEntryWithEmployee(chartEntry, employee));
            }
            FilteredChartEntriesWithEmployees = new ObservableCollection<ChartEntryWithEmployee>(ChartEntriesWithEmployees);
            OnPropertyChanged(nameof(FilteredChartEntriesWithEmployees));
        }

        private async Task filterEntriesAsync()
        {
            var filtered = ChartEntriesWithEmployees
                .Where(e => !IsFilterActive || e.Position == SelectedPosition)
                .Where(e => string.IsNullOrEmpty(SearchText) || (e.FirstName + " " + e.LastName).Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .OrderBy(e => e.StartDate)
                .ThenBy(e => e.Id)
                .ToList();

            FilteredChartEntriesWithEmployees = new ObservableCollection<ChartEntryWithEmployee>(filtered);
            OnPropertyChanged(nameof(FilteredChartEntriesWithEmployees));
        }

        public override async Task InitializeAsync()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now + TimeSpan.FromDays(30);
            EnableDateRange = true;
            await loadEmployeesAsync();
            await loadChartEntriesAsync();
            await filterEntriesAsync();
        }  
    }

    public record ChartEntryWithEmployee
    {
        public int Id { get; set; }
        public Position Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ChartEntryWithEmployee(GetChartEntryDto chartEntry, GetEmployeeDto employee)
        {
            Id = chartEntry.Id;
            Position = employee.Position!.Value;
            StartDate = chartEntry.StartDate;
            EndDate = chartEntry.EndDate;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
        }
    }
}
