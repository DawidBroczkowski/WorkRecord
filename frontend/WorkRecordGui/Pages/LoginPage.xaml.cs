using Microsoft.Extensions.DependencyInjection;
using Plugin.Maui.KeyListener;
using SharpHook;
using WorkRecordGui.Model;
using WorkRecordGui.Pages.Helpers;
using WorkRecordGui.Pages.Models;
using WorkRecordGui.Pages.Models.Helpers;

namespace WorkRecordGui.Pages;

public partial class LoginPage : BasePage
{
    private TaskPoolGlobalHook? _taskPoolGlobalHook;
    public LoginPage(IServiceProvider serviceProvider, IPageModelFactory pageModelFactory) : base(serviceProvider, pageModelFactory)
    {
        InitializeComponent();
        BindingContext = pageModelFactory.CreateViewModel(typeof(LoginPageModel));
    }

    protected override void OnAppearing()
    {
        //_taskPoolGlobalHook = new TaskPoolGlobalHook();
        //_taskPoolGlobalHook.KeyPressed += OnKeyPressed;
        //Task.Run(() => _taskPoolGlobalHook.RunAsync());
        base.OnAppearing();
    }

    //protected override void OnDisappearing()
    //{
    //    _taskPoolGlobalHook!.KeyPressed -= OnKeyPressed;
    //    _taskPoolGlobalHook?.Dispose();
    //    base.OnDisappearing();
    //}

    //private async void OnKeyPressed(object? sender, KeyboardHookEventArgs e)
    //{
    //    if (e.Data.KeyCode is SharpHook.Native.KeyCode.VcEnter)
    //    {
    //        await ((LoginPageModel)BindingContext).LoginAsync();
    //    }
    //}
}


