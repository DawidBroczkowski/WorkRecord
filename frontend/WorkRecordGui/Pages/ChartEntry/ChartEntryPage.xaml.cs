using System.Windows;
using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.ChartEntry;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages;

public partial class ChartEntryPage : BasePage
{
	public ChartEntryPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
		InitializeComponent();
		BindingContext = pageModelFactory.CreateViewModel(typeof(ChartEntryPageModel));
	}

	private async void OnDeleteChartEntryTapped(object sender, EventArgs e)
	{
        if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to delete this chart entry?", "Delete chart entry", MessageBoxButton.YesNo))
		{
            await ((ChartEntryPageModel)BindingContext).DeleteChartEntryAsync();
        }
    }
}