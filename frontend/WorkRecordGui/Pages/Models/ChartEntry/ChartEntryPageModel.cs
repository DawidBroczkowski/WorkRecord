using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.Vacancy;
using WorkRecordGui.Shared.Dtos.ChartEntry;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.Vacancy;

namespace WorkRecordGui.Pages.Models.ChartEntry
{
    public class ChartEntryPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private INavigationService _navigationService;
        private IChartEntryService _chartEntryService;
        private IEmployeeService _employeeService;
        private IVacancyService _vacancyService;
        private GetChartEntryDto _chartEntry;
        private int _chartEntryId;

        public GetChartEntryDto ChartEntry
        {
            get => _chartEntry;
            set
            {
                _chartEntry = value;
                OnPropertyChanged();
            }
        }

        private GetEmployeeDto _employee;
        public GetEmployeeDto Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        private GetEmployeeDto _plannedEmployee;
        public GetEmployeeDto PlannedEmployee
        {
            get => _plannedEmployee;
            set
            {
                _plannedEmployee = value;
                OnPropertyChanged();
            }
        }

        private GetVacancyDto _vacancy;
        public GetVacancyDto Vacancy
        {
            get => _vacancy;
            set
            {
                _vacancy = value;
                OnPropertyChanged();
            }
        }

        public ICommand EmployeeTappedCommand { get; }
        public ICommand VacancyTappedCommand { get; }

        public ChartEntryPageModel(int chartEntryId, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _chartEntryService = _serviceProvider.GetRequiredService<IChartEntryService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();
            EmployeeTappedCommand = new Command<int>(navigateToEmployee);
            VacancyTappedCommand = new Command<int>(navigateToVacancy);
            _chartEntryId = chartEntryId;
        }

        public override async Task InitializeAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            ChartEntry = (await _chartEntryService.GetChartEntryAsync(_chartEntryId, _cts.Token))!;
            Employee = (await _employeeService.GetEmployeeAsync(ChartEntry.EmployeeId, _cts.Token))!;
            if (ChartEntry.VacancyId is not null)
            {
                Vacancy = (await _vacancyService.GetVacancyAsync(ChartEntry.VacancyId.Value, _cts.Token))!;
                if (Vacancy.EmployeeId is not null)
                {
                    PlannedEmployee = (await _employeeService.GetEmployeeAsync(Vacancy.EmployeeId.Value, _cts.Token))!;
                }
            }
        }

        private async void navigateToEmployee(int employeeId)
        {
            await _navigationService.NavigateToAsync(typeof(EmployeePageModel), employeeId);
        }

        private async void navigateToVacancy(int vacancyId)
        {
            await _navigationService.NavigateToAsync(typeof(VacancyPageModel), vacancyId);
        }

        public async Task DeleteChartEntryAsync()
        {
            await _chartEntryService.DeleteChartEntryAsync(ChartEntry.Id, _cts.Token);
            await _navigationService.GoBackAsync();
        }




    }
}
