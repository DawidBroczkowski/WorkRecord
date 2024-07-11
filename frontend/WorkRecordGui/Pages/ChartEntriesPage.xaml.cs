using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages;

public partial class ChartEntriesPage : BasePage
{
	public ChartEntriesPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
	{
		InitializeComponent();
		BindingContext = pageModelFactory.CreateViewModel(typeof(ChartEntriesPageModel));
	}
}