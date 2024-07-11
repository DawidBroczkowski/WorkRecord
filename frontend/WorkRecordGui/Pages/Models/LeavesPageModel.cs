using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.LeaveEntry;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.LeaveEntry;

namespace WorkRecordGui.Pages.Models
{
    class LeavesPageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private ILeaveEntryService _leaveEntryService;
        private IEmployeeService _employeeService;
        private INavigationService _navigationService;
        public ObservableCollection<GetLeaveEntryDto> Leaves { get; set; } = new();
        public ObservableCollection<GetEmployeeDto> Employees { get; set; } = new();
        public ObservableCollection<LeaveEntryWithEmployee> LeavesWithEmployees { get; set; } = new();
        public ObservableCollection<Position> Positions { get; set; }
        private Position _selectedPosition = Position.dentist;
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged();
            }
        }

        public ICommand LeaveTappedCommand { get; }
        public ICommand AddLeaveTappedCommand { get; }

        public LeavesPageModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _leaveEntryService = _serviceProvider.GetRequiredService<ILeaveEntryService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();

            LeaveTappedCommand = new Command<LeaveEntryWithEmployee>(onLeaveTapped);
            AddLeaveTappedCommand = new Command(() => _navigationService.NavigateToAsync(typeof(AddLeavePageModel)));
            Positions = new ObservableCollection<Position>(Enum.GetValues(typeof(Position)).Cast<Position>());
        }

        private async Task loadLeavesAsync()
        {
            try
            {
                var leaves = await _leaveEntryService.GetLeaveEntriesAsync(_cts.Token);
                var employees = await _employeeService.GetEmployeesAsync(_cts.Token);

                Leaves.Clear();
                foreach (var leave in leaves)
                {
                    Leaves.Add(leave);
                }
                leaves.Clear();
                foreach (var employee in employees)
                {
                    Employees.Add(employee);
                }

                LeavesWithEmployees.Clear();
                foreach (var leave in Leaves)
                {
                    var employee = Employees.FirstOrDefault(e => e.Id == leave.EmployeeId);
                    if (employee != null)
                    {
                        LeavesWithEmployees.Add(new LeaveEntryWithEmployee(leave, employee));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void onLeaveTapped(LeaveEntryWithEmployee leave)
        {
            await _navigationService.NavigateToAsync(typeof(LeavePageModel), leave.Id);
        }

        public override async Task InitializeAsync()
        {
            await loadLeavesAsync();
            OnPropertyChanged();
        }
    }

    public record LeaveEntryWithEmployee
    {
        // Leave entry details
        public int Id { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Employee details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Position? Position { get; set; }

        public LeaveEntryWithEmployee(GetLeaveEntryDto leaveEntry, GetEmployeeDto employee)
        {
            // Initialize from leave entry
            Id = leaveEntry.Id;
            LeaveType = leaveEntry.LeaveType;
            StartDate = leaveEntry.StartDate;
            EndDate = leaveEntry.EndDate;

            // Initialize from employee
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Position = employee.Position;
        }
    }
}
