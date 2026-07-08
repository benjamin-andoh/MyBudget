using System;
using System.Collections.Generic;
using System.Text;

namespace MyBudget.Core
{
    public class BudgetService: IBudgetService
    {
        decimal MonthlyLimit { get; set; }

        decimal IBudgetService.MonthlyLimit => MonthlyLimit;

        public void SetMonthlyLimit(decimal limit)
        {
            if (limit <= 0)
            {
                throw new InvalidExpenseException("Monthly limit must be greater than 0.");
            }

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
            decimal threshold = MonthlyLimit * 0.1m;
            if (remaining < 0)
            {
                return BudgetStatus.OverBudget;
            }
            else if (remaining < threshold)
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
