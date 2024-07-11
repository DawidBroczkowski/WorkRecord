using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Vacancy;
namespace WorkRecordGui.Pages;


public partial class EmployeePage : BasePage
{
    private int _employeeId;
    private IServiceProvider _serviceProvider;
    public EmployeePage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(EmployeePageModel));
    }
}