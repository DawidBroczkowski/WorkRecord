using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Employee;

namespace WorkRecordGui.Pages;

public partial class EmployeesPage : BasePage
{
    public EmployeesPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(EmployeesPageModel));
    }

    //void OnRowTapped(object sender, EventArgs args)
    //{
    //    var grid = (Grid)sender;
    //    var employee = (GetEmployeeDto)grid.BindingContext;

    //    Navigation.PushAsync(new EmployeePage(employee.Id, _serviceProvider));
    //}
}