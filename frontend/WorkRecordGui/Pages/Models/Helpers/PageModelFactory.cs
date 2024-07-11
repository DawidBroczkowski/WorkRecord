using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecordGui.Model;
using WorkRecordGui.Models;
using WorkRecordGui.Pages.Models.ChartEntry;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.LeaveEntry;
using WorkRecordGui.Pages.Models.Vacancy;
using WorkRecordGui.Shared.Dtos.ChartEntry;

namespace WorkRecordGui.Pages.Models.Helpers
{
    public class PageModelFactory : IPageModelFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, BaseViewModel> _viewModelCache = new();

        public PageModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BaseViewModel CreateViewModel(Type viewModelType, params object[] parameters)
        {
            if (_viewModelCache.TryGetValue(viewModelType, out var cachedViewModel))
            {
                return cachedViewModel;
            }

            BaseViewModel viewModel;
            if (viewModelType == typeof(AddEmployeePageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else if (viewModelType == typeof(AddLeavePageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else if (viewModelType == typeof(AddVacancyPageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else if (viewModelType == typeof(EmployeePageModel))
            {
                int employeeId = (int)parameters[0];
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, employeeId, _serviceProvider)!;
            }
            else if (viewModelType == typeof(EmployeesPageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else if (viewModelType == typeof(LeavePageModel))
            {
                int leaveEntryId = (int)parameters[0];
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, leaveEntryId, _serviceProvider)!;
            }
            else if (viewModelType == typeof(LeavesPageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else if (viewModelType == typeof(LoginPageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else if (viewModelType == typeof(VacanciesPageModel))
            {
                int weekPlanId = 1;
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, weekPlanId, _serviceProvider)!;
            }
            else if (viewModelType == typeof(VacancyPageModel))
            {
                int vacancyId = (int)parameters[0];
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, vacancyId, _serviceProvider)!;
            }
            else if (viewModelType == typeof(ChartEntriesPageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else if (viewModelType == typeof(EditVacancyPageModel))
            {
                int vacancyId = (int)parameters[0];
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, vacancyId, _serviceProvider)!;
            }
            else if (viewModelType == typeof(ChartEntryPageModel))
            {
                int chartEntryId = (int)parameters[0];
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, chartEntryId, _serviceProvider)!;
            }
            else if (viewModelType == typeof(EditEmployeePageModel))
            {
                int employeeId = (int)parameters[0];
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, employeeId, _serviceProvider)!;
            }
            //else if (viewModelType == typeof(EditLeavePageModel))
            //{
            //    int leaveEntryId = (int)parameters[0];
            //    viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, leaveEntryId, _serviceProvider)!;
            //}
            //else if (viewModelType == typeof(EditChartEntryPageModel))
            //{
            //    int chartEntryId = (int)parameters[0];
            //    viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, chartEntryId, _serviceProvider)!;
            //}
            else if (viewModelType == typeof(AddChartEntryPageModel))
            {
                if (parameters.Length == 2)
                {
                    GetUnfilledChartEntryDto dto = (GetUnfilledChartEntryDto)parameters[0];
                    Position position = (Position)parameters[1];
                    viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, dto, position, _serviceProvider)!;
                }
                else
                {
                    viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
                }
            }
            else if (viewModelType == typeof(UnfilledChartEntriesPageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else if (viewModelType == typeof(ReportPageModel))
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType, _serviceProvider)!;
            }
            else
            {
                viewModel = (BaseViewModel)Activator.CreateInstance(viewModelType)!;
            }

            _viewModelCache[viewModelType] = viewModel;
            return viewModel;
        }

        public void ClearViewModel(Type viewModelType)
        {
            _viewModelCache.Remove(viewModelType);
        }
    }
}
