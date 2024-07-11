using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models.ChartEntry;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages.ChartEntry;

public partial class AddChartEntryPage : BasePage
{
	public AddChartEntryPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(AddChartEntryPageModel));
    }
}