using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Pages;
using WorkRecordGui.Pages.ChartEntry;
using WorkRecordGui.Pages.Employee;

namespace WorkRecordGui
{
    public partial class AppShell : Shell
    {
        private INavigationService _navigationService;
        private IServiceProvider _serviceProvider;

        public AppShell(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            InitializeComponent();
            Routing.RegisterRoute("//Root/Login", typeof(LoginPage));
            Routing.RegisterRoute("//Root/Employees", typeof(EmployeesPage));
            Routing.RegisterRoute("//Root/Main", typeof(MainPage));
            Routing.RegisterRoute("//Root/Vacancies", typeof(VacanciesPage));
            Routing.RegisterRoute("//Root/AddEmployee", typeof(AddEmployeePage));
            Routing.RegisterRoute("//Root/AddVacancy", typeof(AddVacancyPage));
            Routing.RegisterRoute("//Root/AddLeave", typeof(AddLeavePage));
            Routing.RegisterRoute("//Root/Leave", typeof(LeavePage));
            Routing.RegisterRoute("//Root/Leaves", typeof(LeavesPage));
            Routing.RegisterRoute("//Root/Employee", typeof(EmployeePage));
            Routing.RegisterRoute("//Root/Vacancy", typeof(VacancyPage));
            Routing.RegisterRoute("//Root/ChartEntries", typeof(ChartEntriesPage));
            Routing.RegisterRoute("//Root/EditVacancy", typeof(EditVacancyPage));
            Routing.RegisterRoute("//Root/ChartEntry", typeof(ChartEntryPage));
            Routing.RegisterRoute("//Root/EditEmployee", typeof(EditEmployeePage));
            Routing.RegisterRoute("//Root/AddChartEntry", typeof(AddChartEntryPage));
            Routing.RegisterRoute("//Root/UnfilledChartEntries", typeof(UnfilledChartEntriesPage));
            Routing.RegisterRoute("//Root/Report", typeof(ReportPage));
        }

        //protected override async void OnNavigating(ShellNavigatingEventArgs args)
        //{
        //    // for some reason, when pressing the go back arrow in shell, the source is push, so this doesn't work
        //    if (args.Source == ShellNavigationSource.Pop)
        //    {
        //        args.Cancel(); // Cancel the default back navigation
        //        await _navigationService.GoBackAsync(); // Call your custom navigation method
        //    }
        //    else
        //    {
        //        base.OnNavigating(args);
        //    }
        //}
    }
}
