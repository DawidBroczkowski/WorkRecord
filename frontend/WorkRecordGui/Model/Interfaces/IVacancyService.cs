using WorkRecordGui.Models;
using WorkRecordGui.Shared.Dtos.Vacancy;

namespace WorkRecordGui.Model.Interfaces
{
    public interface IVacancyService
    {
        Task AddVacancyAsync(CreateVacancyDto createVacancyDto, CancellationToken cancellationToken);
        Task ChangeVacancyStatusAsync(int id, bool isActive, CancellationToken cancellationToken);
        Task DeleteVacancyAsync(int id, CancellationToken cancellationToken);
        Task<List<GetVacancyDto>> GetVacanciesAsync(CancellationToken cancellationToken);
        Task<List<GetVacancyDto>> GetVacanciesByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<List<GetVacancyDto>> GetVacanciesByIsActiveAsync(bool isActive, CancellationToken cancellationToken);
        Task<List<GetVacancyDto>> GetVacanciesByOccurrenceDayAsync(DayOfWeek occurrenceDay, CancellationToken cancellationToken);
        Task<List<GetVacancyDto>> GetVacanciesByPlannedEmployeeIdAsync(int plannedEmployeeId, CancellationToken cancellationToken);
        Task<List<GetVacancyDto>> GetVacanciesByPositionAsync(Position position, CancellationToken cancellationToken);
        Task<List<GetVacancyDto>> GetVacanciesByWeekPlanAndPositionAsync(int weekPlanId, Position position, CancellationToken cancellationToken);
        Task<List<GetVacancyDto>> GetVacanciesByWeekPlanIdAsync(int weekPlanId, CancellationToken cancellationToken);
        Task<GetVacancyDto?> GetVacancyAsync(int id, CancellationToken cancellationToken);
        Task UpdateVacancyAsync(UpdateVacancyDto updateVacancyDto, CancellationToken cancellationToken);
        Task<bool> VacancyExistsAsync(int id, CancellationToken cancellationToken);
    }
}