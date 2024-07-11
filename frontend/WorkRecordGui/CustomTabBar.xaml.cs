using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Employee;

namespace WorkRecordGui;

public partial class CustomTabBar : ContentView, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly INavigationService _navigationService;
    private readonly Session _session;
    public Session Session 
    {   get
        {
            return _session;
        } 
    }

    public CustomTabBar()
    {
        InitializeComponent();
        this.BindingContext = this;
        _navigationService = (INavigationService)Application.Current!.MainPage!.Handler!.MauiContext!.Services.GetService(typeof(INavigationService))!;
        _session = (Session)Application.Current!.MainPage!.Handler!.MauiContext!.Services.GetService(typeof(Session))!;
        OnPropertyChanged(nameof(Session));
    }

    private async void OnGoBackClicked(object sender, EventArgs e)
    {
        if (_navigationService.GetStackSize() > 0)
        {
            await _navigationService.GoBackAsync();
        }
    }

    private async void OnEmployeesClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(typeof(EmployeesPageModel));
    }

    private async void OnVacanciesClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(typeof(VacanciesPageModel),1);
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(typeof(LoginPageModel));
    }

    private async void OnLeavesClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(typeof(LeavesPageModel));
    }

    private async void OnChartEntriesClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(typeof(ChartEntriesPageModel));
    }

    private async void OnUnfilledEntriesClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(typeof(UnfilledChartEntriesPageModel));
    }

    private async void OnReportClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(typeof(ReportPageModel));
    }

    private async void OnMyProfileClicked(object sender, EventArgs e)
    {
        await _navigationService.NavigateToAsync(typeof(EmployeePageModel), _session.User.EmployeeId!);
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}