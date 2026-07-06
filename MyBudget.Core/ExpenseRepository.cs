using System;
using System.Collections.Generic;
using System.Text;

namespace MyBudget.Core
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly List<Expense> _expenses;
        private readonly IExpenseStore _expenseStore;

        public ExpenseRepository(IExpenseStore store) 
        {
            _expenseStore = store;
            _expenses = store.Load().ToList();
        }


        public void Add(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            _expenses.Add(expense);
        }

        public IReadOnlyList<Expense> GetAll()
        {
            return _expenses.OrderBy(e=>e.Date).ToList();
        }

        public IReadOnlyList<Expense> InCategory(ExpenseCategory category)
        {
            return _expenses.Where(e => e.Category == category).ToList();
        }

        public void Save()
        {
            _expenseStore.Save(_expenses);
        }

        public decimal Total()
        {
            return _expenses.Sum(e => e.MonthlyImpact);
        }

        public IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory()
        {
            return _expenses
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.MonthlyImpact));  
        }
    }
}
