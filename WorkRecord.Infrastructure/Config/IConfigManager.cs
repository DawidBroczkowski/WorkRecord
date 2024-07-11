namespace WorkRecord.Infrastructure.Config
{
    public interface IConfigManager
    {
        int GetWeekPlanId();
        void SetWeekPlanId(int id);
        int GetPlanUpdateSeconds();
        void SetPlanUpdateSeconds(int seconds);
    }
}