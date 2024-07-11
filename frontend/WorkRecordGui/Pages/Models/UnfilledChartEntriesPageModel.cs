using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.ChartEntry;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.ChartEntry;
using WorkRecordGui.Shared.Dtos.Employee;

namespace WorkRecordGui.Pages.Models
{
    public class UnfilledChartEntriesPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IChartEntryService _chartEntryService;
        private INavigationService _navigationService;
        private IEmployeeService _employeeService;
        private IPlanManagerService _planManagerService;
        private IVacancyService _vacancyService;

        private List<GetUnfilledChartEntryDto> _chartEntries;
        private List<GetEmployeeDto> _employees;
        public ObservableCollection<GetEmployeeDto> Employees { get; set; } = new();
        public List<GetUnfilledChartEntryDto> ChartEntries { get; set; } = new();
        public ObservableCollection<GetUnfilledChartEntryDto> FilteredChartEntries { get; set; } = new();
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

        public Position SelectedPosition { get; set; } = Position.dentist;

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

        public ICommand EntryTappedCommand => new Command<GetUnfilledChartEntryDto>(async (dto) =>
        {
            Position position = (await _vacancyService.GetVacancyAsync(dto.VacancyId, _cts.Token))!.Position!.Value;
            await _navigationService.NavigateToAsync(typeof(AddChartEntryPageModel), dto, position);
        });

        public ICommand AddEntryCommand => new Command(async () =>
        {
            await _navigationService.NavigateToAsync(typeof(AddChartEntryPageModel));
        });

        public ICommand GetEntriesCommand => new Command(async () =>
        {
            await loadUnfilledChartEntriesAsync();
        });

        public UnfilledChartEntriesPageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _chartEntryService = _serviceProvider.GetRequiredService<IChartEntryService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _planManagerService = _serviceProvider.GetRequiredService<IPlanManagerService>();
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();

            Positions = new ObservableCollection<Position>(Enum.GetValues(typeof(Position)).Cast<Position>());
        }

        //private async Task loadChartEntriesAsync()
        //{
        //    var entries = await _chartEntryService.GetChartEntriesByDateRangeAndPositionAsync(StartDate!.Value, EndDate!.Value, SelectedPosition, _cts.Token);
        //}

        //private async Task loadEmployeesAsync()
        //{
        //    var employees = await _employeeService.GetEmployeesAsync(_cts.Token);
        //    Employees = new ObservableCollection<GetEmployeeDto>(employees);
        //    OnPropertyChanged(nameof(Employees));
        //}

        private async Task loadUnfilledChartEntriesAsync()
        {
            ChartEntries = await _planManagerService.GetUnfilledVacanciesAsync(StartDate!.Value, EndDate!.Value, _cts.Token);
            FilteredChartEntries = new ObservableCollection<GetUnfilledChartEntryDto>(ChartEntries);
            OnPropertyChanged(nameof(FilteredChartEntries));
        }

        private async Task filterEntriesAsync()
        {
            var filtered = ChartEntries
                .Where(e => !IsFilterActive || e.Position == SelectedPosition)
                .OrderBy(e => e.StartDate)
                .ToList();

            FilteredChartEntries = new ObservableCollection<GetUnfilledChartEntryDto>(filtered);
            OnPropertyChanged(nameof(FilteredChartEntries));
        }

        //private void filterEmployees()
        //{
        //    var filtered = Employees
        //        .Where(e => string.IsNullOrEmpty(SearchText) || (e.FirstName + " " + e.LastName).Contains(SearchText, StringComparison.OrdinalIgnoreCase))
        //        .ToList();
        //}

        public override async Task InitializeAsync()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now + TimeSpan.FromDays(30);
            EnableDateRange = true;
            //await loadEmployeesAsync();
            //await loadChartEntriesAsync();
            await loadUnfilledChartEntriesAsync();
            await filterEntriesAsync();
        }
    }
}
