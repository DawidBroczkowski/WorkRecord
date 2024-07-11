namespace WorkRecord.Shared.Dtos.WeekPlan
{
    public record GetWeekPlanDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
