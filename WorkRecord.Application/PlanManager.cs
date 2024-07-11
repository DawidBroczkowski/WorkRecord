using Microsoft.Extensions.DependencyInjection;
using System.Transactions;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.Config;
using WorkRecord.Infrastructure.DataAccess;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared.Dtos.ChartEntry;

namespace WorkRecord.Application
{
     public class PlanManager : IPlanManager
     {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public PlanManager(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InitializeAsync()
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();
                await SetCurrentPlanAsync(configManager.GetWeekPlanId());
                DateTime currentDate = DateTime.Now;
                DateTime nextMonth = currentDate.AddMonths(1);
                await UpdateFutureEntriesAsync(currentDate, nextMonth, _cts.Token);
            }
        }

        public async Task SetCurrentPlanAsync(int id)
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var weekPlanRepository = scope.ServiceProvider.GetRequiredService<IWeekPlanRepository>();
                var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();

                if (await weekPlanRepository.WeekPlanExistsAsync(id, _cts.Token) is false)
                {
                    var ex = new KeyNotFoundException("Week plan with the given id does not exist");
                    ex.Data.Add("Id", id);
                    throw new Exception("Failed to set current plan", ex);
                }
                configManager.SetWeekPlanId(id);
            }
        }

        public int GetCurrentPlanId()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();
                return configManager.GetWeekPlanId();
            }
        }

        public async Task UpdateFutureEntriesAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var vacancyRepository = scope.ServiceProvider.GetRequiredService<IVacancyRepository>();
                var chartEntryRepository = scope.ServiceProvider.GetRequiredService<IChartEntryRepository>();
                var leaveEntryRepository = scope.ServiceProvider.GetRequiredService<ILeaveEntryRepository>();
                var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();
                var transactionManager = scope.ServiceProvider.GetRequiredService<ITransactionManager>();

                var vacancies = await vacancyRepository.GetActiveVacanciesByWeekPlanIdAsync(configManager.GetWeekPlanId(), cancellationToken);
                List<int> employeeIds = vacancies
                    .Where(v => v.EmployeeId.HasValue)
                    .Select(v => v.EmployeeId!.Value)
                    .Distinct()
                    .ToList();

                List<CreateChartEntryDto> dtos = new();

                foreach (int id in employeeIds)
                {
                    var chartEntries = await chartEntryRepository.GetChartEntriesByDateOverlapAndEmployeeIdAsync(from, to, id, cancellationToken);
                    var employeeVacancies = vacancies.FindAll(v => v.EmployeeId == id).OrderBy(x => x.OccurrenceDay);
                    var employeeLeaves = await leaveEntryRepository.GetLeaveEntriesByEmployeeIdAsync(id, from: from, cancellationToken);

                    foreach (var vacancy in employeeVacancies)
                    {
                        Position position = vacancy.Position!.Value;
                        DateTime nextDate = from;
                        while (nextDate.DayOfWeek != vacancy.OccurrenceDay)
                        {
                            nextDate = nextDate.AddDays(1);
                        }

                        while (nextDate < to)
                        {
                            DateTime startDate = DateTime.Parse($"{nextDate.ToShortDateString()} {vacancy.StartHour}");
                            DateTime endDate = DateTime.Parse($"{nextDate.ToShortDateString()} {vacancy.EndHour}");

                            if (endDate < from)
                            {
                                nextDate = nextDate.AddDays(7);
                                continue;
                            }

                            if (chartEntries.Exists(c => c.StartDate == startDate && c.EndDate >= endDate) is false
                                && employeeLeaves.Exists(l => l.StartDate <= nextDate && l.EndDate >= nextDate) is false)
                            {
                                if (employeeLeaves.Exists(l => l.StartDate <= startDate && l.EndDate >= endDate) is true)
                                {
                                    nextDate = nextDate.AddDays(7);
                                    continue;
                                }
                                else if (employeeLeaves.FirstOrDefault(l => l.StartDate <= startDate && l.EndDate >= startDate) is not null)
                                {
                                    startDate = employeeLeaves.First(l => l.StartDate <= startDate && l.EndDate >= startDate).EndDate;
                                }
                                else if (employeeLeaves.FirstOrDefault(l => l.StartDate <= endDate && l.EndDate >= endDate) is not null)
                                {
                                    endDate = employeeLeaves.First(l => l.StartDate <= startDate && l.EndDate >= startDate).StartDate;
                                }

                                dtos.Add(new CreateChartEntryDto
                                {
                                    EmployeeId = id,
                                    StartDate = startDate,
                                    EndDate = endDate,
                                    VacancyId = vacancy.Id
                                });
                            }
                            nextDate = nextDate.AddDays(7);
                        }
                    }
                }

                var transaction = await transactionManager.BeginTransactionAsync();
                try
                {
                    await chartEntryRepository.BulkAddChartEntryAsync(dtos, cancellationToken);
                }
                catch (Exception e)
                {
                    await transactionManager.RollbackTransactionAsync(transaction);
                    throw new Exception("Failed to update future entries", e);
                }
                await transactionManager.CommitTransactionAsync(transaction);
            }
        }

        //public async Task<List<GetUnfilledChartEntryDto>> GetUnfilledVacanciesAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        //{
        //    using (var scope = _serviceScopeFactory.CreateAsyncScope())
        //    {
        //        var vacancyRepository = scope.ServiceProvider.GetRequiredService<IVacancyRepository>();
        //        var chartEntryRepository = scope.ServiceProvider.GetRequiredService<IChartEntryRepository>();
        //        var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();
        //        var vacancies = await vacancyRepository.GetActiveVacanciesByWeekPlanIdAsync(configManager.GetWeekPlanId(), cancellationToken);
        //        var chartEntries = await chartEntryRepository.GetChartEntriesByDateOverlapAsync(from, to, cancellationToken);
        //        List<GetUnfilledChartEntryDto> unfilledChartEntries = new();

        //        foreach (var vacancy in vacancies)
        //        {
        //            var vacancyChartEntries = chartEntries.FindAll(c => c.VacancyId == vacancy.Id);
        //            DateTime nextDate = from;
        //            while (nextDate <= to)
        //            {
        //                if (vacancyChartEntries.Exists(c => c.StartDate <= nextDate && c.EndDate >= nextDate) is false)
        //                {
        //                    unfilledChartEntries.Add(new GetUnfilledChartEntryDto
        //                    {
        //                        StartDate = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, vacancy.StartHour.Hours, vacancy.StartHour.Minutes, vacancy.StartHour.Seconds),
        //                        EndDate = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, vacancy.EndHour.Hours, vacancy.EndHour.Minutes, vacancy.EndHour.Seconds),
        //                        Position = vacancy.Position,
        //                        VacancyId = vacancy.Id
        //                    });
        //                }
        //                nextDate = nextDate.AddDays(7);
        //            }
        //        }

        //        return unfilledChartEntries;
        //    }
        //}

        public async Task<List<GetUnfilledChartEntryDto>> GetUnfilledVacanciesAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var vacancyRepository = scope.ServiceProvider.GetRequiredService<IVacancyRepository>();
                var chartEntryRepository = scope.ServiceProvider.GetRequiredService<IChartEntryRepository>();
                var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();

                var vacancies = await vacancyRepository.GetActiveVacanciesByWeekPlanIdAsync(configManager.GetWeekPlanId(), cancellationToken);
                var chartEntries = await chartEntryRepository.GetChartEntriesByDateOverlapAsync(from, to, cancellationToken);
                List<GetUnfilledChartEntryDto> unfilledChartEntries = new();

                foreach (var vacancy in vacancies)
                {
                    DateTime nextDate = from;
                    // Find the first occurrence of the vacancy's day of the week within the date range
                    while (nextDate.DayOfWeek != vacancy.OccurrenceDay)
                    {
                        nextDate = nextDate.AddDays(1);
                    }

                    while (nextDate <= to)
                    {
                        DateTime vacancyStartDate = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, vacancy.StartHour.Hours, vacancy.StartHour.Minutes, vacancy.StartHour.Seconds);
                        DateTime vacancyEndDate = new DateTime(nextDate.Year, nextDate.Month, nextDate.Day, vacancy.EndHour.Hours, vacancy.EndHour.Minutes, vacancy.EndHour.Seconds);

                        var overlappingChartEntries = chartEntries.Where(c => c.VacancyId == vacancy.Id && c.StartDate < vacancyEndDate && c.EndDate > vacancyStartDate).ToList();

                        if (overlappingChartEntries.Exists(c => c.StartDate == vacancyStartDate && c.EndDate == vacancyEndDate))
                        {
                            nextDate = nextDate.AddDays(7);
                            continue;
                        }
                        if (overlappingChartEntries.Count == 0)
                        {
                            unfilledChartEntries.Add(new GetUnfilledChartEntryDto
                            {
                                StartDate = vacancyStartDate,
                                EndDate = vacancyEndDate,
                                Position = vacancy.Position,
                                VacancyId = vacancy.Id
                            });
                            nextDate = nextDate.AddDays(7);
                            continue;
                        }

                        List<(DateTime Start, DateTime End)> unfilledPeriods = new() { (vacancyStartDate, vacancyEndDate) };

                        // Remove periods covered by chart entries
                        foreach (var chartEntry in overlappingChartEntries)
                        {
                            var newUnfilledPeriods = new List<(DateTime Start, DateTime End)>();

                            foreach (var period in unfilledPeriods)
                            {
                                if (chartEntry.StartDate >= period.End || chartEntry.EndDate <= period.Start)
                                {
                                    newUnfilledPeriods.Add(period);
                                }
                                else
                                {
                                    if (chartEntry.StartDate > period.Start)
                                    {
                                        newUnfilledPeriods.Add((period.Start, chartEntry.StartDate));
                                    }
                                    if (chartEntry.EndDate < period.End)
                                    {
                                        newUnfilledPeriods.Add((chartEntry.EndDate, period.End));
                                    }
                                }
                            }
                            unfilledPeriods = newUnfilledPeriods;
                        }

                        // Add all remaining unfilled periods to the result list
                        foreach (var period in unfilledPeriods)
                        {
                            unfilledChartEntries.Add(new GetUnfilledChartEntryDto
                            {
                                StartDate = period.Start,
                                EndDate = period.End,
                                Position = vacancy.Position,
                                VacancyId = vacancy.Id
                            });
                        }

                        nextDate = nextDate.AddDays(7);
                    }
                }

                return unfilledChartEntries;
            }
        }

        public async Task ChangeCurrentPlanAsync(int planId, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateAsyncScope())
            {
                var weekPlanRepository = scope.ServiceProvider.GetRequiredService<IWeekPlanRepository>();
                var chartEntryRepository = scope.ServiceProvider.GetRequiredService<IChartEntryRepository>();
                var transactionManager = scope.ServiceProvider.GetRequiredService<ITransactionManager>();
                var configManager = scope.ServiceProvider.GetRequiredService<IConfigManager>();

                if (await weekPlanRepository.WeekPlanExistsAsync(planId, _cts.Token) is false)
                {
                    var ex = new KeyNotFoundException("Week plan with the given id does not exist");
                    ex.Data.Add("Id", planId);
                    throw new Exception("Failed to set current plan", ex);
                }
                var chartEntries = await chartEntryRepository.GetChartEntriesByDateOverlapAsync(DateTime.Now, DateTime.MaxValue, cancellationToken);
                var transaction = await transactionManager.BeginTransactionAsync();
                try
                {
                    await chartEntryRepository.BulkDeleteChartEntriesAsync(chartEntries, cancellationToken);
                    await weekPlanRepository.ChangeCurrentWeekPlanAsync(configManager.GetWeekPlanId(), planId, cancellationToken);
                }
                catch (Exception e)
                {
                    await transactionManager.RollbackTransactionAsync(transaction);
                    throw new Exception("Failed to change current plan", e);
                }
                await transactionManager.CommitTransactionAsync(transaction);

                configManager.SetWeekPlanId(planId);
                await UpdateFutureEntriesAsync(DateTime.Now, DateTime.Now.AddMonths(1), cancellationToken);
            }
        }
    }
}
