using System.Windows;
using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.Vacancy;
using WorkRecordGui.Shared.Dtos.Employee;
using WorkRecordGui.Shared.Dtos.Vacancy;

namespace WorkRecordGui.Pages;

public partial class VacancyPage : BasePage
{
    public VacancyPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(VacancyPageModel));
    }

    private async void OnDeleteVacancyTapped(object sender, EventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to delete this vacancy?", "Delete vacancy", MessageBoxButton.YesNo))
        {
            await ((VacancyPageModel)BindingContext).DeleteVacancyAsync();
        }
    }

    //private void OnPlannedEmployeeFrameTapped(object sender, EventArgs e)
    //{
    //    var frame = sender as Frame;
    //    var vacancyDto = frame?.BindingContext as GetVacancyDto;
    //    var employeeId = vacancyDto!.PlannedEmployeeId;
    //    Navigation.PushAsync(new EmployeePage(employeeId, _serviceProvider));
    //}
}