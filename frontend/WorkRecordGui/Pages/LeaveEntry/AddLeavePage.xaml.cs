using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.LeaveEntry;

namespace WorkRecordGui.Pages;

public partial class AddLeavePage : BasePage
{
    public AddLeavePage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = _pageModelFactory.CreateViewModel(typeof(AddLeavePageModel));
    }
}