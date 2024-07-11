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
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.Vacancy;

namespace WorkRecordGui.Pages.Models.Vacancy
{
    public class EditVacancyPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IVacancyService _vacancyService;
        private IEmployeeService _employeeService;
        private INavigationService _navigationService;
        public ObservableCollection<GetEmployeeDto> Employees { get; set; } = new();
        public ObservableCollection<Position> Positions { get; set; }
        private Position _selectedPosition = Position.dentist;
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                _updateVacancy.Position = value;
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
                _updateVacancy.OccurrenceDay = value;
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
                _updateVacancy.PlannedEmployeeId = value?.Id ?? 0;
                OnPropertyChanged();
                // Trigger the list to refresh and reapply the filter to move the selected employee to the top
                SearchText = _searchText;
            }
        }
        private TimeSpan _startHour = new(8, 0, 0);
        public TimeSpan StartHour
        {
            get => _startHour;
            set
            {
                _startHour = value;
                _updateVacancy.StartHour = value;
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
                _updateVacancy.EndHour = value;
                OnPropertyChanged();
            }
        }

        private UpdateVacancyDto _updateVacancy = new();

        public ICommand EditVacancyTappedCommand { get; }
        public ICommand EmployeeSelectedCommand { get; }

        public EditVacancyPageModel(int id, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();

            Positions = new ObservableCollection<Position>(Enum.GetValues(typeof(Position)).Cast<Position>());
            DaysOfWeek = new ObservableCollection<DayOfWeek>(Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>());
            _serviceProvider = serviceProvider;

            _updateVacancy.Id = id;

            EditVacancyTappedCommand = new Command(editVacancyTapped);
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

        private async void editVacancyTapped()
        {
            await _vacancyService.UpdateVacancyAsync(_updateVacancy, _cts.Token);
            await _navigationService.NavigateToAsync(typeof(VacancyPageModel), _updateVacancy.Id);
        }

        private async Task loadVacancyAsync()
        {
            var vacancy = await _vacancyService.GetVacancyAsync(_updateVacancy.Id, _cts.Token);
            _updateVacancy.StartHour = vacancy!.StartHour;
            _updateVacancy.EndHour = vacancy.EndHour;
            _updateVacancy.Position = vacancy.Position;
            _updateVacancy.OccurrenceDay = vacancy.OccurrenceDay;
            _updateVacancy.IsActive = vacancy.IsActive;
            _updateVacancy.PlannedEmployeeId = vacancy.PlannedEmployeeId;
            OnPropertyChanged(nameof(Vacancy));
        }

        public override async Task InitializeAsync()
        {
            await loadVacancyAsync();
            await loadEmployees();
            var employees = Employees.Where(e => e.Position == SelectedPosition).ToList();
            FilteredEmployees.Clear();
            foreach (var employee in employees)
            {
                FilteredEmployees.Add(employee);
            }
            OnPropertyChanged(nameof(FilteredEmployees));
        }

        private void onEmployeeSelected(GetEmployeeDto employee)
        {
            SelectedEmployee = employee;
        }
    }
}
