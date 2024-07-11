using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.Vacancy;
using WorkRecordGui.Shared.Dtos.WeekPlan;

namespace WorkRecordGui.Pages.Models.Vacancy
{
    public class AddVacancyPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IVacancyService _vacancyService;
        private IEmployeeService _employeeService;
        private INavigationService _navigationService;
        private IWeekPlanService _weekPlanService;
        public ObservableCollection<GetEmployeeDto> Employees { get; set; } = new();
        public ObservableCollection<Position> Positions { get; set; }
        private Position _selectedPosition = Position.dentist;
        public ObservableCollection<WeekPlanViewModel> WeekPlans { get; set; } = new();


        private WeekPlanViewModel _selectedWeekPlan = new WeekPlanViewModel(new GetWeekPlanDto());
        public WeekPlanViewModel SelectedWeekPlan
        {
            get => _selectedWeekPlan;
            set
            {
                if (value is not null)
                {
                    _selectedWeekPlan = value;
                    Vacancy.WeekPlanId = value.WeekPlan.Id;
                }
                OnPropertyChanged();
            }
        }

        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                Vacancy.Position = value;
                var filtered = Employees
                    .Where(e => (string.IsNullOrEmpty(SearchText) || (e.FirstName + " " + e.LastName).Contains(SearchText, StringComparison.OrdinalIgnoreCase)) && e.Position == SelectedPosition)
                    .OrderBy(e => e != SelectedEmployee) // This ensures the selected employee is always at the top
                    .ToList();

                FilteredEmployees = new ObservableCollection<GetEmployeeDto>(filtered);
                OnPropertyChanged(nameof(FilteredEmployees));

                OnPropertyChanged();
            }
        }
        public ObservableCollection<DayOfWeek> DaysOfWeek { get; set; }
        private DayOfWeek _selectedDayOfWeek = DayOfWeek.Monday;
        public DayOfWeek SelectedDayOfWeek
        {
            get => _selectedDayOfWeek;
            set
            {
                _selectedDayOfWeek = value;
                Vacancy.OccurrenceDay = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GetEmployeeDto> FilteredEmployees { get; set; } = new();
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                var filtered = Employees
                    .Where(e => (string.IsNullOrEmpty(value) || (e.FirstName + " " + e.LastName).Contains(value, StringComparison.OrdinalIgnoreCase)) && e.Position == SelectedPosition)
                    .OrderBy(e => e != SelectedEmployee) // This ensures the selected employee is always at the top
                    .ToList();

                FilteredEmployees = new ObservableCollection<GetEmployeeDto>(filtered);
                OnPropertyChanged(nameof(FilteredEmployees));
            }
        }

        private GetEmployeeDto _selectedEmployee = new();
        public GetEmployeeDto SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                Vacancy.PlannedEmployeeId = value?.Id ?? 0;
                SearchText = _searchText;
                if (SelectedEmployee.Id != 0)
                {
                    Task.Run(loadVacanciesAsync);
                }
                OnPropertyChanged();
            }
        }
        private TimeSpan _startHour = new(8, 0, 0);
        public TimeSpan StartHour
        {
            get => _startHour;
            set
            {
                _startHour = value;
                Vacancy.StartHour = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _endHour = new(16, 0, 0);
        public TimeSpan EndHour
        {
            get => _endHour;
            set
            {
                _endHour = value;
                Vacancy.EndHour = value;
                OnPropertyChanged();
            }
        }

        private CreateVacancyDto _vacancy = new();
        public CreateVacancyDto Vacancy
        {
            get => _vacancy;
            set
            {
                _vacancy = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GetVacancyDto> Vacancies { get; set; } = new();


        public ICommand AddVacancyTappedCommand { get; }
        public ICommand EmployeeSelectedCommand { get; }

        public AddVacancyPageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _weekPlanService = _serviceProvider.GetRequiredService<IWeekPlanService>();

            Positions = new ObservableCollection<Position>(Enum.GetValues(typeof(Position)).Cast<Position>());
            DaysOfWeek = new ObservableCollection<DayOfWeek>(Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>());

            FilteredEmployees = Employees;
            _serviceProvider = serviceProvider;

            AddVacancyTappedCommand = new Command(addVacancyTapped);
            EmployeeSelectedCommand = new Command<GetEmployeeDto>(onEmployeeSelected);
        }

        private async Task loadEmployees()
        {
            try
            {
                var employees = await _employeeService.GetEmployeesAsync(_cts.Token);
                Employees.Clear();
                foreach (var employee in employees)
                {
                    Employees.Add(employee);
                }
                OnPropertyChanged(nameof(Employees));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task loadWeekPlansAsync()
        {
            try
            {
                var weekPlans = await _weekPlanService.GetWeekPlansAsync(_cts.Token);
                WeekPlans = new ObservableCollection<WeekPlanViewModel>(weekPlans.Select(w => new WeekPlanViewModel(w)));
                SelectedWeekPlan = WeekPlans.FirstOrDefault()!;
                OnPropertyChanged(nameof(WeekPlans));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task loadVacanciesAsync()
        {
            try
            {
                var vacancies = await _vacancyService.GetVacanciesByEmployeeIdAsync(SelectedEmployee.Id, _cts.Token);
                Vacancies = new ObservableCollection<GetVacancyDto>(vacancies);
                OnPropertyChanged(nameof(Vacancies));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void addVacancyTapped()
        {
            await _vacancyService.AddVacancyAsync(Vacancy, _cts.Token);
            await _navigationService.NavigateToAsync(typeof(VacanciesPageModel));
        }

        public override async Task InitializeAsync()
        {
            await loadEmployees();
            var employees = Employees.Where(e => e.Position == SelectedPosition).ToList();
            FilteredEmployees.Clear();
            foreach (var employee in employees)
            {
                FilteredEmployees.Add(employee);
            }
            await loadWeekPlansAsync();
            OnPropertyChanged(nameof(FilteredEmployees));
        }

        private void onEmployeeSelected(GetEmployeeDto employee)
        {
            SelectedEmployee = employee;
        }
    }

    public class WeekPlanViewModel
    {
        public GetWeekPlanDto WeekPlan { get; }

        public WeekPlanViewModel(GetWeekPlanDto weekPlan)
        {
            WeekPlan = weekPlan;
        }

        public string DisplayText => $"{WeekPlan.Id} - {WeekPlan.Name}";
    }
}
