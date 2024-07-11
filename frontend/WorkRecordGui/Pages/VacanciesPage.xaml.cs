using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Shared.Dtos.Vacancy;

namespace WorkRecordGui.Pages;

public partial class VacanciesPage : BasePage
{
    public VacanciesPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(VacanciesPageModel));
    }

    //private void OnFrameTapped(object sender, EventArgs e)
    //{
    //    var frame = sender as Frame;
    //    var vacancyDto = frame?.BindingContext as GetVacancyDto;
    //    var vacancyId = vacancyDto!.Id;
    //    Navigation.PushAsync(new VacancyPage(vacancyId, _serviceProvider));
    //}
}