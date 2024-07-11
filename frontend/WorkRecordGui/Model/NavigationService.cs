using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Pages;
using WorkRecordGui.Pages.ChartEntry;
using WorkRecordGui.Pages.Employee;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.ChartEntry;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.LeaveEntry;
using WorkRecordGui.Pages.Models.Vacancy;

namespace WorkRecordGui.Model
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<Type, Type> _mappings; // Maps ViewModels to Views
        private readonly Stack<(Type ViewModelType, object[] Parameters)> _navigationStack = new(); // Stack to keep track of pages and parameters
        private IPageModelFactory _pageModelFactory;
        private IServiceProvider _serviceProvider;

        public NavigationService(IPageModelFactory viewModelFactory, IServiceProvider serviceProvider)
        {
            _mappings = new Dictionary<Type, Type>();
            Register<AddEmployeePageModel, AddEmployeePage>();
            Register<AddLeavePageModel, AddLeavePage>();
            Register<AddVacancyPageModel, AddVacancyPage>();
            Register<EmployeePageModel, EmployeePage>();
            Register<EmployeesPageModel, EmployeesPage>();
            Register<LeavePageModel, LeavePage>();
            Register<LeavesPageModel, LeavesPage>();
            Register<LoginPageModel, LoginPage>();
            Register<VacanciesPageModel, VacanciesPage>();
            Register<VacancyPageModel, VacancyPage>();
            Register<ChartEntriesPageModel, ChartEntriesPage>();
            Register<EditVacancyPageModel, EditVacancyPage>();
            Register<ChartEntryPageModel, ChartEntryPage>();
            Register<EditEmployeePageModel, EditEmployeePage>();
            Register<AddChartEntryPageModel, AddChartEntryPage>();
            Register<UnfilledChartEntriesPageModel, UnfilledChartEntriesPage>();
            Register<ReportPageModel, ReportPage>();
            _pageModelFactory = viewModelFactory;
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToAsync(Type pageType, params object[] parameters)
        {
            Type viewType = _mappings[pageType];
            _pageModelFactory.CreateViewModel(pageType, parameters);
            string route = $"{viewType.Name.Replace("Page", "")}";
            _navigationStack.Push((pageType, parameters)); // Push the view model type and parameters to the stack

             await Shell.Current.GoToAsync(route);
        }

        public async Task GoBackAsync()
        {
            if (_navigationStack.Count > 0)
            {
                _navigationStack.Pop(); // Remove the current page from the stack
                if (_navigationStack.Count > 0)
                {
                    var (viewModelType, parameters) = _navigationStack.Peek();
                    Type viewType = _mappings[viewModelType];
                    var viewModel = _pageModelFactory.CreateViewModel(viewModelType, parameters); // Use the non-generic method
                    string route = $"//Root/{viewType.Name.Replace("Page", "")}";
                    await Shell.Current.GoToAsync(route);

                    // Optionally clear the ViewModel cache to ensure fresh instances
                    _pageModelFactory.ClearViewModel(viewModelType);
                }
                else
                {
                    await Shell.Current.GoToAsync("//Root");
                }
            }
            else
            {
                await Shell.Current.GoToAsync("//Root");
            }
        }

        public Type GetMappedViewModel(Type viewType)
        {
            // Find the first entry in the mappings dictionary where the value matches the provided viewType.
            // This returns a KeyValuePair<Type, Type> where the key is the ViewModel type and the value is the View type.
            var entry = _mappings.FirstOrDefault(x => x.Value == viewType);

            return entry.Key;
        }

        public int GetStackSize()
        {
            return _navigationStack.Count;
        }

        protected void Register<TViewModel, TView>() where TViewModel : BaseViewModel where TView : Page
        {
            _mappings[typeof(TViewModel)] = typeof(TView);
        }
    }
}
