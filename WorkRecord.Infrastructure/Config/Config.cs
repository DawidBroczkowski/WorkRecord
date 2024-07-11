using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkRecord.Infrastructure.Config
{
    public record Config
    {
        public int WeekPlanId { get; set; }
        public int PlanUpdateSeconds { get; set; }
    }
}
