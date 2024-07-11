using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.LeaveEntry;
using WorkRecordGui.Pages.Models.Vacancy;
using WorkRecordGui.Shared.Dtos.ChartEntry;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.LeaveEntry;
using WorkRecordGui.Shared.Dtos.Vacancy;

namespace WorkRecordGui.Pages.Models.Employee
{
    public class EmployeePageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IEmployeeService _employeeService;
        private IVacancyService _vacancyService;
        private ILeaveEntryService _leaveEntryService;
        private INavigationService _navigationService;
        private IChartEntryService _chartEntryService;

        private int _employeeId;
        private GetEmployeeDto? _employee;
        public GetEmployeeDto Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GetVacancyDto> Vacancies { get; set; }
        public ObservableCollection<GetLeaveEntryDto> ScheduledLeaves { get; set; }
        public ObservableCollection<GetChartEntryDto> ChartEntries { get; set; }

        public ICommand VacancyTappedCommand { get; }
        public ICommand LeaveTappedCommand { get; }
        public ICommand EditEmployeeTappedCommand { get; }
        public ICommand DeleteEmployeeTappedCommand { get; }
        public ICommand AddChildTappedCommand { get; }


        public EmployeePageModel(int employeeId, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _employeeId = employeeId;
            _serviceProvider = serviceProvider;
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();
            _leaveEntryService = _serviceProvider.GetRequiredService<ILeaveEntryService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _chartEntryService = _serviceProvider.GetRequiredService<IChartEntryService>();
            VacancyTappedCommand = new Command<int>(navigateToVacancy);
            LeaveTappedCommand = new Command<int>(navigateToLeave);
            EditEmployeeTappedCommand = new Command(editEmployee);
            AddChildTappedCommand = new Command(addChild);
        }

        private async Task loadEmployeeAsync()
        {
            try
            {
                Employee = (await _employeeService.GetEmployeeAsync(_employeeId, _cts.Token))!;
            }
            catch (Exception e)
            {
                Employee = null!;
            }
            OnPropertyChanged(nameof(Employee));
        }

        private async Task loadVacanciesAsync()
        {
            try
            {
                var vacancies = await _vacancyService.GetVacanciesByEmployeeIdAsync(_employeeId, _cts.Token);
                Vacancies = new ObservableCollection<GetVacancyDto>(vacancies);
            }
            catch (Exception e)
            {
                Vacancies = null!;
            }
            OnPropertyChanged(nameof(Vacancies));
        }

        private async Task loadScheduledLeavesAsync()
        {
            try
            {
                var leaves = await _leaveEntryService.GetLeaveEntriesByEmployeeIdAsync(_employeeId, _cts.Token);
                var futureLeaves = leaves.Where(l => l.EndDate > DateTime.Now);
                ScheduledLeaves = new ObservableCollection<GetLeaveEntryDto>(futureLeaves);
            }
            catch (Exception e)
            {
                ScheduledLeaves = null!;
            }
            OnPropertyChanged(nameof(ScheduledLeaves));
        }

        private async Task loadChartEntriesAsync()
        {
            var entries = await _chartEntryService.GetChartEntriesByDateOverlapAndEmployeeIdAsync(DateTime.Now, DateTime.MaxValue, _employeeId, _cts.Token);
            ChartEntries = new ObservableCollection<GetChartEntryDto>(entries);
            OnPropertyChanged(nameof(ChartEntries));
        }

        private async void navigateToVacancy(int vacancyId)
        {
            await _navigationService.NavigateToAsync(typeof(VacancyPageModel), vacancyId);
        }

        private async void navigateToLeave(int leaveId)
        {
            await _navigationService.NavigateToAsync(typeof(LeavePageModel), leaveId);
        }

        private async void editEmployee()
        {
            await _navigationService.NavigateToAsync(typeof(EditEmployeePageModel), _employeeId);
        }

        private async void addChild()
        {
            await _employeeService.AddChildAsync(_employeeId, Date, _cts.Token);
            await loadEmployeeAsync();
        }

        public override async Task InitializeAsync()
        {
            await loadEmployeeAsync();
            await loadVacanciesAsync();
            await loadScheduledLeavesAsync();
            await loadChartEntriesAsync();
        }
    }
}
