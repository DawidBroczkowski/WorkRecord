using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkRecord.Infrastructure.Config;
using WorkRecord.Infrastructure.DataAccess;
using WorkRecord.Infrastructure.DataAccess.Interfaces;

namespace WorkRecord.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WorkRecordContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default"),
                b => b.MigrationsAssembly(typeof(WorkRecordContext).Assembly.FullName)), ServiceLifetime.Transient);

            services.AddScoped<IUserRepository, DbUserRepository>();
            services.AddScoped<IChartEntryRepository, DbChartEntryRepository>();
            services.AddScoped<IVacancyRepository, DbVacancyRepository>();
            services.AddScoped<ILeaveEntryRepository, DbLeaveEntryRepository>();
            services.AddScoped<IWeekPlanRepository, DbWeekPlanRepository>();
            services.AddScoped<IEmployeeRepository, DbEmployeeRepository>();
            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped<IWeekPlanRepository, DbWeekPlanRepository>();

            IConfigManager config = new ConfigManager("config.json");
            services.AddSingleton<IConfigManager>(config);
            return services;
        }

        public static IServiceProvider EnsureDatabaseIsCreated(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WorkRecordContext>();
                dbContext.Database.EnsureCreated();
            }

            return services;
        }
    }
}
