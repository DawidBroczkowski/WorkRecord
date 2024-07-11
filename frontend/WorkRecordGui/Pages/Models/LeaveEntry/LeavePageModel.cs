using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.LeaveEntry;

namespace WorkRecordGui.Pages.Models.LeaveEntry
{
    public class LeavePageModel : BaseViewModel
    {
        private IServiceProvider _serviceProvider;
        private ILeaveEntryService _leaveEntryService;
        private IEmployeeService _employeeService;
        private INavigationService _navigationService;
        private int _leaveEntryId;

        private GetLeaveEntryDto? _leave;
        public GetLeaveEntryDto? Leave
        {
            get => _leave;
            set
            {
                _leave = value;
                OnPropertyChanged();
            }
        }

        private GetEmployeeDto? _employee;
        public GetEmployeeDto? Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                OnPropertyChanged();
            }
        }

        public LeavePageModel(int leaveEntryId, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _leaveEntryService = _serviceProvider.GetRequiredService<ILeaveEntryService>();
            _employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            _leaveEntryId = leaveEntryId;
        }

        private async Task loadDataAsync(int leaveEntryId)
        {
            try
            {
                Leave = (await _leaveEntryService.GetLeaveEntryAsync(leaveEntryId, _cts.Token))!;
                Employee = (await _employeeService.GetEmployeeAsync(Leave.EmployeeId, _cts.Token))!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public override async Task InitializeAsync()
        {
            await loadDataAsync(_leaveEntryId);
        }

        public async Task DeleteLeaveAsync()
        {
            try
            {
                await _leaveEntryService.DeleteLeaveEntryAsync(_leaveEntryId, _cts.Token);
                await _navigationService.GoBackAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
