using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.Vacancy;

namespace WorkRecordGui.Pages;

public partial class EditVacancyPage : BasePage
{
    public EditVacancyPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = _pageModelFactory.CreateViewModel(typeof(EditVacancyPageModel));
    }
}