using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages;

public partial class AddEmployeePage : BasePage
{
    public AddEmployeePage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(AddEmployeePageModel));
    }
}