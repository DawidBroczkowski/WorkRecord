using System.Windows.Forms;
using WorkRecordGui.Model;
using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages;

public partial class ReportPage : BasePage
{
	public ReportPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
		BindingContext = pageModelFactory.CreateViewModel(typeof(ReportPageModel));
	}
}