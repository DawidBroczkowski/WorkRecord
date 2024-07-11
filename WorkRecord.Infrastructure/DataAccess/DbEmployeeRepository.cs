using Microsoft.EntityFrameworkCore;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.DataAccess.Interfaces;
using WorkRecord.Shared;
using WorkRecord.Shared.Dtos.Employee;

namespace WorkRecord.Infrastructure.DataAccess
{
    public class DbEmployeeRepository : IEmployeeRepository
    {
        private WorkRecordContext _db;

        public DbEmployeeRepository(WorkRecordContext db)
        {
            _db = db;
        }

        public async Task<GetEmployeeDto?> GetEmployeeByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Employees
                .IgnoreAutoIncludes()
                .Where(e => e.Id == id)
                .Select(e => e.AsDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<GetEmployeeDto?> GetEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _db.Employees
                .IgnoreAutoIncludes()
                .Where(e => e.Email == email)
                .Select(e => e.AsDto())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddEmployeeAsync(CreateEmployeeDto dto, CancellationToken cancellationToken)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PESEL = dto.PESEL,
                BirthDate = dto.BirthDate,
                Position = dto.Position,
                YearsWorked = dto.YearsWorked
            };

            _db.Employees.Add(employee);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EmployeeExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.Employees.AnyAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<bool> EmployeeExistsAsync(string email, CancellationToken cancellationToken)
        {
            return await _db.Employees.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<List<GetEmployeeDto>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            return await _db.Employees
                .IgnoreAutoIncludes()
                .Select(u => u.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FindAsync(dto.Id, cancellationToken);

            employee!.FirstName = dto.FirstName ?? employee.FirstName;
            employee.LastName = dto.LastName ?? employee.LastName;
            employee.Email = dto.Email ?? employee.Email;
            employee.PhoneNumber = dto.PhoneNumber ?? employee.PhoneNumber;
            employee.PESEL = dto.PESEL ?? employee.PESEL;
            employee.BirthDate = dto.BirthDate ?? employee.BirthDate;
            employee.Position = dto.Position ?? employee.Position;
            employee.YearsWorked = dto.YearsWorked ?? employee.YearsWorked;
            employee.PaidLeaveDays = dto.PaidLeaveDays ?? employee.PaidLeaveDays;
            employee.OnDemandLeaveDays = dto.OnDemandLeaveDays ?? employee.OnDemandLeaveDays;
            employee.PreviousYearPaidLeaveDays = dto.PreviousYearPaidLeaveDays ?? employee.PreviousYearPaidLeaveDays;
            employee.ChildcareHours = dto.ChildcareHours ?? employee.ChildcareHours;
            employee.HigherPowerHours = dto.HigherPowerHours ?? employee.HigherPowerHours;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteEmployeeAsync(int id, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FindAsync(id, cancellationToken);
            foreach (var entry in employee!.ChartEntries)
            {
                entry.Employee = null;
            }
            _db.Employees.Remove(employee!);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<GetEmployeeDto>> GetEmployeesByPositionAsync(Position position, CancellationToken cancellationToken)
        {
            return await _db.Employees
                .IgnoreAutoIncludes()
                .Where(e => e.Position == position)
                .Select(e => e.AsDto())
                .ToListAsync(cancellationToken);
        }

        public async Task AddChildAsync(int employeeId, DateTime birthday, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FindAsync(employeeId, cancellationToken);
            employee!.ChildrenBirthdays.Add(birthday);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveChildAsync(int employeeId, ushort index, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FindAsync(employeeId, cancellationToken);
            employee!.ChildrenBirthdays.RemoveAt(index);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<DateTime>> GetChildrenBirthdaysAsync(int employeeId, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FindAsync(employeeId, cancellationToken);
            return employee!.ChildrenBirthdays;
        }

        public async Task<int> GetChildrenCountAsync(int employeeId, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FindAsync(employeeId, cancellationToken);
            return employee!.ChildrenBirthdays.Count;
        }

        public async Task<GetLeaveDaysDto> GetLeaveDaysAsync(int employeeId, CancellationToken cancellationToken)
        {
            var employee = await _db.Employees.FindAsync(employeeId, cancellationToken);
            return new GetLeaveDaysDto
            {
                PaidLeaveDays = employee!.PaidLeaveDays,
                OnDemandLeaveDays = employee.OnDemandLeaveDays,
                PreviousYearPaidLeaveDays = employee.PreviousYearPaidLeaveDays,
                ChildcareHours = employee.ChildcareHours,
                HigherPowerHours = employee.HigherPowerHours
            };
        }

        public async Task BulkUpdateEmployeesAsync(List<UpdateEmployeeDto> dtos, CancellationToken cancellationToken)
        {;
            var employees = await _db.Employees
                .Where(e => dtos.Select(dto => dto.Id).Contains(e.Id))
                .ToListAsync(cancellationToken);
                
            foreach (var employee in employees)
            {
                var dto = dtos.First(d => d.Id == employee.Id);
                employee.FirstName = dto.FirstName ?? employee.FirstName;
                employee.LastName = dto.LastName ?? employee.LastName;
                employee.Email = dto.Email ?? employee.Email;
                employee.PhoneNumber = dto.PhoneNumber ?? employee.PhoneNumber;
                employee.PESEL = dto.PESEL ?? employee.PESEL;
                employee.BirthDate = dto.BirthDate ?? employee.BirthDate;
                employee.Position = dto.Position ?? employee.Position;
                employee.YearsWorked = dto.YearsWorked ?? employee.YearsWorked;
                employee.PaidLeaveDays = dto.PaidLeaveDays ?? employee.PaidLeaveDays;
                employee.OnDemandLeaveDays = dto.OnDemandLeaveDays ?? employee.OnDemandLeaveDays;
                employee.PreviousYearPaidLeaveDays = dto.PreviousYearPaidLeaveDays ?? employee.PreviousYearPaidLeaveDays;
                employee.ChildcareHours = dto.ChildcareHours ?? employee.ChildcareHours;
                employee.HigherPowerHours = dto.HigherPowerHours ?? employee.HigherPowerHours;
            }
        }
    }
}
