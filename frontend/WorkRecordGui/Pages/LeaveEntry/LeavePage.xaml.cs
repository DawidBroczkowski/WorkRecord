using System.Windows;
using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Models.LeaveEntry;

namespace WorkRecordGui.Pages;

public partial class LeavePage : BasePage
{
    public LeavePage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(LeavePageModel));
    }

    private async void OnDeleteLeaveTapped(object sender, EventArgs e)
    {
        if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to delete this leave entry?", "Delete leave entry", MessageBoxButton.YesNo))
        {
            await ((LeavePageModel)BindingContext).DeleteLeaveAsync();
        }
    }
}