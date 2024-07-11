using Microsoft.Extensions.Logging;
using Plugin.Maui.KeyListener;
using System.Net.Http.Headers;
using WorkRecordGui.Model;
using WorkRecordGui.Pages;
using WorkRecordGui.Model.Interfaces;
using WorkRecordGui.Pages.Models.Helpers;
using WorkRecordGui.Pages.Employee;
using WorkRecordGui.Pages.ChartEntry;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;

namespace WorkRecordGui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseKeyListener()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            Session session = new();
            builder.Services.AddSingleton<Session>(session);
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IVacancyService, VacancyService>();
            builder.Services.AddTransient<ILeaveEntryService, LeaveEntryService>();
            builder.Services.AddTransient<IEmployeeService, EmployeeService>();
            builder.Services.AddTransient<IPlanManagerService, PlanManagerService>();
            builder.Services.AddTransient<IWeekPlanService, WeekPlanService>();
            builder.Services.AddTransient<IChartEntryService, ChartEntryService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IPageModelFactory, PageModelFactory>();
            builder.Services.AddTransient<IReportService, ReportService>();
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<AuthorizationHandler>();
            builder.Services.AddSingleton<CustomTabBar>();
            builder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);

            // Register pages
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<AddEmployeePage>();
            builder.Services.AddTransient<AddVacancyPage>();
            builder.Services.AddTransient<AddLeavePage>();
            builder.Services.AddTransient<EmployeePage>();
            builder.Services.AddTransient<EmployeesPage>();
            builder.Services.AddTransient<LeavePage>();
            builder.Services.AddTransient<LeavesPage>();
            builder.Services.AddTransient<VacanciesPage>();
            builder.Services.AddTransient<VacancyPage>();
            builder.Services.AddTransient<ChartEntriesPage>();
            builder.Services.AddTransient<EditVacancyPage>();
            builder.Services.AddTransient<ChartEntryPage>();
            builder.Services.AddTransient<EditEmployeePage>();
            builder.Services.AddTransient<AddChartEntryPage>();
            builder.Services.AddTransient<UnfilledChartEntriesPage>();
            builder.Services.AddTransient<ReportPage>();

            builder.Services.AddHttpClient("User", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/User/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AuthorizationHandler>();

            builder.Services.AddHttpClient("JWT", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/User/login/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            });

            builder.Services.AddHttpClient("Vacancy", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/Vacancy/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AuthorizationHandler>();

            builder.Services.AddHttpClient("ChartEntry", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/ChartEntry/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AuthorizationHandler>();

            builder.Services.AddHttpClient("LeaveEntry", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/LeaveEntry/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AuthorizationHandler>();

            builder.Services.AddHttpClient("Employee", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/Employee/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AuthorizationHandler>();

            builder.Services.AddHttpClient("PlanManager", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/PlanManager/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AuthorizationHandler>();

            builder.Services.AddHttpClient("WeekPlan", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/WeekPlan/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AuthorizationHandler>();

            builder.Services.AddHttpClient("Report", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7079/api/Report/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }).AddHttpMessageHandler<AuthorizationHandler>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
