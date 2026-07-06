using System;
using System.Collections.Generic;
using System.Text;

namespace MyBudget.Core
{
    internal class BudgetService
    {
        decimal MonthlyLimit { get; set; }

        public void SetMonthlyLimit(decimal limit)
        {
            MonthlyLimit = limit;
        }

        public decimal Remaining(decimal totalSpent)
        {
            return MonthlyLimit - totalSpent;
        }   

        public BudgetStatus Evaluate(decimal totalSpent)
        {
            if (MonthlyLimit == 0)
            {
                return BudgetStatus.NotSet;
            }
            decimal remaining = Remaining(totalSpent);
            decimal threshold = MonthlyLimit * 0.1m; // 10% of the monthly limit
            if (remaining < 0)
            {
                return BudgetStatus.OverBudget;
            }
            else if (remaining <= threshold)
            {
                return BudgetStatus.AlmostOut;
            }
            else
            {
                return BudgetStatus.OnTrack;
            }
        }   

    }
}
