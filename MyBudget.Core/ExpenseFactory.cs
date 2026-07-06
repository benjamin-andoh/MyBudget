using static MyBudget.Core.Expense;

namespace MyBudget.Core
{
    public static class ExpenseFactory
    {
        public static Expense Create(string description, decimal amount, ExpenseCategory category, DateOnly date, int? timesPerMonth = null)
        {
            if (timesPerMonth.HasValue)
            {
                return CreateRecurring(description, amount, category, date, timesPerMonth.Value);
            }
            else
            {
                return CreateOneTime(description, amount, category, date);
            }
        }

        public static decimal ValidateAmount(decimal amount)
        {
            if (amount <= 0 || amount > 1000000)
            {
                throw new InvalidExpenseException("amount cannot be less than 0 or greather than 1,000,000");
            }
            return Math.Round(amount, 2);
        }

        public static OneTimeExpense CreateOneTime(string description, decimal amount, ExpenseCategory category, DateOnly date)
        {
            ValidateDescription(description);
            return new OneTimeExpense(Guid.NewGuid(), description, ValidateAmount(amount), category, date);
        }

        public static RecurringExpense CreateRecurring(string description, decimal amount,
                                  ExpenseCategory category, DateOnly date, int timesPerMonth)
        {
            ValidateDescription(description);

            if (timesPerMonth < 1)
            {
                throw new InvalidExpenseException("Times per month must be at least 1.");
            }

            return new RecurringExpense(
                Guid.NewGuid(),
                description.Trim(),
                ValidateAmount(amount),
                category,
                date,
                timesPerMonth);
        }
        private static void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new InvalidExpenseException(
                    "Description cannot be blank.");
            }
        }
    }
}
