using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.Vacancy;

namespace WorkRecordGui.Pages.Models.Vacancy
{
    class VacancyPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private INavigationService _navigationService;
        private IVacancyService _vacancyService;
        private IEmployeeService _employeeService;
        private GetVacancyDto _vacancy;
        private int _vacancyId;

        public GetVacancyDto Vacancy
        {
            get => _vacancy;
            set
            {
                _vacancy = value;
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

        public ICommand EmployeeTappedCommand { get; }
        public ICommand ChangeActiveStatusTappedCommand { get; }
        public ICommand EditVacancyTappedComand { get; }
        //public ICommand DeleteVacancyTappedCommand { get; }

        public VacancyPageModel(int vacancyId, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _vacancyService = _serviceProvider.GetRequiredService<IVacancyService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            EmployeeTappedCommand = new Command<int>(navigateToEmployee);
            ChangeActiveStatusTappedCommand = new Command(changeActiveStatus);
            EditVacancyTappedComand = new Command(editVacancy);
            //DeleteVacancyTappedCommand = new Command(deleteVacancy);
            _vacancyId = vacancyId;
        }

        private async Task loadDataAsync(int id)
        {
            try
            {
                Vacancy = (await _vacancyService.GetVacancyAsync(id, _cts.Token))!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                if (Vacancy.EmployeeId is not null)
                {
                    PlannedEmployee = (await _employeeService.GetEmployeeAsync(Vacancy.EmployeeId.Value, _cts.Token))!;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void navigateToEmployee(int id)
        {
            await _navigationService.NavigateToAsync(typeof(EmployeePageModel), id);
        }

        private async void changeActiveStatus()
        {
            await _vacancyService.ChangeVacancyStatusAsync(Vacancy.Id, !Vacancy.IsActive, _cts.Token);
            await loadDataAsync(Vacancy.Id);
        }

        private async void editVacancy()
        {
            await _navigationService.NavigateToAsync(typeof(EditVacancyPageModel), Vacancy.Id);
        }

        public async Task DeleteVacancyAsync()
        {
            await _vacancyService.DeleteVacancyAsync(Vacancy.Id, _cts.Token);
            await _navigationService.GoBackAsync();
        }

        public override async Task InitializeAsync()
        {
            await loadDataAsync(_vacancyId);
            OnPropertyChanged();
        }
    }
}
