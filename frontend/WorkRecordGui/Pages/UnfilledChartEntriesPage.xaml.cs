using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages;

public partial class UnfilledChartEntriesPage : BasePage
{
	public UnfilledChartEntriesPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
	{
		InitializeComponent();
		BindingContext = pageModelFactory.CreateViewModel(typeof(UnfilledChartEntriesPageModel));
	}
}