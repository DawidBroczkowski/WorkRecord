using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages;

public partial class LeavesPage : BasePage
{
    public LeavesPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(LeavesPageModel));
    }
}