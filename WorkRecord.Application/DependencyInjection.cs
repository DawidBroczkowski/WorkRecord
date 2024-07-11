using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices.Marshalling;
using WorkRecord.Application.Services;
using WorkRecord.Application.Services.Interfaces;
using WorkRecord.Infrastructure;

namespace WorkRecord.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IChartEntryService, ChartEntryService>();
            services.AddTransient<IVacancyService, VacancyService>();
            services.AddTransient<IWeekPlanService, WeekPlanService>();
            services.AddTransient<ILeaveEntryService, LeaveEntryService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IPlanManager, PlanManager>();
            services.AddTransient<IReportService, ReportService>();
            return services;
        }

        public static void Configure(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IPlanManager planManager)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () => await planManager.InitializeAsync());
            });
        }
    }
}
