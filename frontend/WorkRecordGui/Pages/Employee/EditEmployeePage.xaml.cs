using System.Windows;
using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.Employee;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.Vacancy;

namespace WorkRecordGui.Pages.Employee;

public partial class EditEmployeePage : BasePage
{
	public EditEmployeePage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = _pageModelFactory.CreateViewModel(typeof(EditEmployeePageModel));
    }

    private async void OnUpdateEmployeeTapped(object sender, EventArgs e)
    {
        if (((EditEmployeePageModel)BindingContext).GetEmployee.Position != ((EditEmployeePageModel)BindingContext).Employee.Position)
        {             
            if (MessageBoxResult.Yes == MessageBox.Show("This will delete every vacancy associated with this " +
                "employee. Do you want to continue?", "Update employee", MessageBoxButton.YesNo))
            {
                await ((EditEmployeePageModel)BindingContext).UpdateEmployee();
            }   
        }
        else
        {
            await ((EditEmployeePageModel)BindingContext).UpdateEmployee();
        }
    }
}