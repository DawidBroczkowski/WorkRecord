using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Employee;

namespace WorkRecordGui.Pages.Models.Employee
{
    public class EditEmployeePageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private IEmployeeService _employeeService;
        private INavigationService _navigationService;
        public ObservableCollection<Position> Positions { get; set; }
        private Position _selectedPosition = Position.dentist;
        private int _employeeId;
        private GetEmployeeDto _getEmployee = new();
        public GetEmployeeDto GetEmployee
        {
            get => _getEmployee;
            set
            {
                _getEmployee = value;
                OnPropertyChanged();
            }
        }

        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                Employee.Position = value;
                OnPropertyChanged();
            }
        }

        private UpdateEmployeeDto _employee = new();
        public UpdateEmployeeDto Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public EditEmployeePageModel(int id, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Positions = new ObservableCollection<Position>(Enum.GetValues(typeof(Position)).Cast<Position>());
            _serviceProvider = serviceProvider;
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _employeeId = id;
        }

        public override async Task InitializeAsync()
        {
            await loadEmployeeAsync();
        }

        public async Task UpdateEmployee()
        {
            var dto = new UpdateEmployeeDto
            {
                Id = Employee.Id,
                FirstName = Employee.FirstName == _getEmployee.FirstName ? null : Employee.FirstName,
                LastName = Employee.LastName == _getEmployee.LastName ? null : Employee.LastName,
                Email = Employee.Email == _getEmployee.Email ? null : Employee.Email,
                PhoneNumber = Employee.PhoneNumber == _getEmployee.PhoneNumber ? null : Employee.PhoneNumber,
                PESEL = Employee.PESEL == _getEmployee.PESEL ? null : Employee.PESEL,
                BirthDate = Employee.BirthDate == _getEmployee.BirthDate ? null : Employee.BirthDate,
                Position = Employee.Position == _getEmployee.Position ? null : Employee.Position,
                ChildrenBirthdays = Employee.ChildrenBirthdays == _getEmployee.ChildrenBirthdays ? null : Employee.ChildrenBirthdays,
                YearsWorked = Employee.YearsWorked == _getEmployee.YearsWorked ? null : Employee.YearsWorked,
                PaidLeaveDays = Employee.PaidLeaveDays == _getEmployee.PaidLeaveDays ? null : Employee.PaidLeaveDays,
                OnDemandLeaveDays = Employee.OnDemandLeaveDays == _getEmployee.OnDemandLeaveDays ? null : Employee.OnDemandLeaveDays,
                PreviousYearPaidLeaveDays = Employee.PreviousYearPaidLeaveDays == _getEmployee.PreviousYearPaidLeaveDays ? null : Employee.PreviousYearPaidLeaveDays,
                ChildcareHours = Employee.ChildcareHours == _getEmployee.ChildcareHours ? null : Employee.ChildcareHours,
                HigherPowerHours = Employee.HigherPowerHours == _getEmployee.HigherPowerHours ? null : Employee.HigherPowerHours
            };
            try
            {
                await _employeeService.UpdateEmployeeAsync(dto, _cts.Token);
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task loadEmployeeAsync()
        {
            try
            {
                _getEmployee = (await _employeeService.GetEmployeeAsync(_employeeId, _cts.Token))!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Employee = new UpdateEmployeeDto
            {
                Id = _getEmployee.Id,
                FirstName = _getEmployee.FirstName,
                LastName = _getEmployee.LastName,
                Email = _getEmployee.Email,
                PhoneNumber = _getEmployee.PhoneNumber,
                PESEL = _getEmployee.PESEL,
                BirthDate = _getEmployee.BirthDate,
                Position = _getEmployee.Position,
                ChildrenBirthdays = _getEmployee.ChildrenBirthdays,
                YearsWorked = _getEmployee.YearsWorked,
                PaidLeaveDays = _getEmployee.PaidLeaveDays,
                OnDemandLeaveDays = _getEmployee.OnDemandLeaveDays,
                PreviousYearPaidLeaveDays = _getEmployee.PreviousYearPaidLeaveDays,
                ChildcareHours = _getEmployee.ChildcareHours,
                HigherPowerHours = _getEmployee.HigherPowerHours
            };
            SelectedPosition = _getEmployee.Position!.Value;
            
        }
        
    }
}
