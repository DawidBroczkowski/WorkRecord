using WorkRecord.Domain.Models;
using WorkRecord.Shared.Dtos.ChartEntry;
using WorkRecord.Shared.Dtos.Employee;
using WorkRecord.Shared.Dtos.LeaveEntry;
using WorkRecord.Shared.Dtos.User;
using WorkRecord.Shared.Dtos.Vacancy;
using WorkRecord.Shared.Dtos.WeekPlan;

namespace WorkRecord.Shared
{
    public static class Extensions
    {
        public static GetUserDto AsGetUserDto(this User user)
        {
            return new GetUserDto
            {
                Id = user.Id,
                Login = user.Login,
                EmployeeId = user.EmployeeId
            };
        }

        public static GetLeaveEntryDto AsDto(this LeaveEntry leaveEntry)
        {
            return new GetLeaveEntryDto
            {
                Id = leaveEntry.Id,
                StartDate = leaveEntry.StartDate,
                EndDate = leaveEntry.EndDate,
                LeaveType = leaveEntry.LeaveType,
                EmployeeId = leaveEntry.Employee!.Id
            };
        }

        public static GetCredentialsDto AsGetCredentialsDto(this User user)
        {
            return new GetCredentialsDto
            {
                Login = user.Login,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };
        }

        public static GetChartEntryDto AsDto(this ChartEntry chartEntry)
        {
            return new GetChartEntryDto
            {
                Id = chartEntry.Id,
                StartDate = chartEntry.StartDate,
                EndDate = chartEntry.EndDate,
                EmployeeId = chartEntry.EmployeeId,
                VacancyId = chartEntry.VacancyId
            };
        }

        public static GetEmployeeDto AsDto(this Employee employee)
        {
            return new GetEmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                PESEL = employee.PESEL,
                BirthDate = employee.BirthDate,
                Position = employee.Position,
                ChildrenBirthdays = employee.ChildrenBirthdays,
                PaidLeaveDays = employee.PaidLeaveDays,
                OnDemandLeaveDays = employee.OnDemandLeaveDays,
                PreviousYearPaidLeaveDays = employee.PreviousYearPaidLeaveDays,
                ChildcareHours = employee.ChildcareHours,
                HigherPowerHours = employee.HigherPowerHours,
                YearsWorked = employee.YearsWorked
            };
        }

        public static GetVacancyDto AsDto(this Vacancy vacancy)
        {
            return new GetVacancyDto
            {
                Id = vacancy.Id,
                StartHour = vacancy.StartHour,
                EndHour = vacancy.EndHour,
                Position = vacancy.Position,
                OccurrenceDay = vacancy.OccurrenceDay,
                IsActive = vacancy.IsActive,
                EmployeeId = vacancy.EmployeeId,
                PlannedEmployeeId = vacancy.PlannedEmployeeId
            };
        }

        public static GetWeekPlanDto AsDto(this WeekPlan weekPlan)
        {
            return new GetWeekPlanDto
            {
                Id = weekPlan.Id,
                Name = weekPlan.Name
            };
        }
    }
}