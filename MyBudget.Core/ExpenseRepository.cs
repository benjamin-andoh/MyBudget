using System;
using System.Collections.Generic;
using System.Text;

namespace MyBudget.Core
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly List<Expense> _expense;
        private readonly IExpenseStore _expenseStore;

        public ExpenseRepository(IExpenseStore store) 
        {
            _expenseStore = store;
            _expense = store.Load().ToList();
        }


        public void Add(Expense expense)
        {
            if (expense == null)
            {
                throw new ArgumentNullException(nameof(expense));
            }
            _expense.Add(expense);
        }

        public IReadOnlyList<Expense> GetAll()
        {
            return _expense.OrderBy(e=>e.Date).ToList();
        }

        public IReadOnlyList<Expense> InCategory(ExpenseCategory category)
        {
            return _expense.Where(e => e.Category == category).ToList();
        }

        public void Save()
        {
            _expenseStore.Save(_expense);
        }

        public decimal Total()
        {
            return _expense.Sum(e => e.MonthlyImpact);
        }

        public IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory()
        {
            return _expense
                .GroupBy(e => e.Category)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.MonthlyImpact));  
        }
    }
}
