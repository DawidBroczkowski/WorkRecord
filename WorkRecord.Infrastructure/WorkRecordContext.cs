using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkRecord.Domain.Models;

namespace WorkRecord.Infrastructure
{
    public class WorkRecordContext : DbContext
    {
        public WorkRecordContext(DbContextOptions<WorkRecordContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ChartEntry> ChartEntries { get; set; }
        public DbSet<LeaveEntry> LeaveEntries { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WeekPlan> WeekPlans { get; set; }
        public DbSet<Vacancy> Vacancies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Employee>()
                .HasMany(e => e.ChartEntries)
                .WithOne(ce => ce.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Entity<Employee>()
                .HasMany(e => e.LeaveEntries)
                .WithOne(le => le.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Entity<ChartEntry>()
                .HasOne(ce => ce.Employee)
                .WithMany(e => e.ChartEntries)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ChartEntry>()
                .HasOne(ce => ce.Vacancy)
                .WithMany(e => e.ChartEntries)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder
                .Entity<LeaveEntry>()
                .HasOne(le => le.Employee)
                .WithMany(e => e.LeaveEntries)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Employee>()
                .Property(e => e.ChildrenBirthdays)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                    v => JsonSerializer.Deserialize<List<DateTime>>(v, JsonSerializerOptions.Default)!
                );

            builder
                .Entity<Employee>()
                .Property(e => e.ChildrenBirthdays)
                .Metadata.SetValueComparer(new ValueComparer<List<DateTime>>
                ((c1, c2) => c1!.SequenceEqual(c2!), c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), c => c.ToList()));

            builder
                .Entity<Vacancy>()
                .HasOne(v => v.WeekPlan)
                .WithMany(wp => wp.Vacancies)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
