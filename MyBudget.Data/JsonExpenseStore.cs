using System.Text.Json;
using MyBudget.Core;

namespace MyBudget.Data;

public class JsonExpenseStore : IExpenseStore
{
    public string DataPath { get; }

    public JsonExpenseStore(string dataPath)
    {
        DataPath = dataPath;
    }

    public IReadOnlyList<Expense> Load()
    {
        if (!File.Exists(DataPath))
        {
            return new List<Expense>();
        }

        string json = File.ReadAllText(DataPath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Expense>();
        }

        List<Expense>? expenses =
            JsonSerializer.Deserialize<List<Expense>>(json);

        return expenses ?? new List<Expense>();
    }

    public void Save(IEnumerable<Expense> expenses)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(expenses, options);

        File.WriteAllText(DataPath, json);
    }
}