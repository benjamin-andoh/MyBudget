using MyBudget.Core;
using Xunit;

namespace MyBudget.Tests;

public class BudgetServiceAdditionalTests
{
    [Fact]
    public void SetMonthlyLimit_StoresLimitSuccessfully()
    {
        var service = new BudgetService();
        service.SetMonthlyLimit(1000m);
        Assert.Equal(BudgetStatus.OnTrack, service.Evaluate(0m));
    }

    [Fact]
    public void Remaining_EqualsLimit_WhenNothingSpent()
    {
        var service = new BudgetService();
        service.SetMonthlyLimit(750m);
        decimal remaining = service.Remaining(0m);
        Assert.Equal(750m, remaining);
    }

    [Theory]
    [InlineData(500, 0)]
    [InlineData(450, 50)]
    [InlineData(300, 200)]
    [InlineData(100, 400)]
    public void Remaining_ReturnsExpectedValue(decimal spent, decimal expected)
    {
        var service = new BudgetService();
        service.SetMonthlyLimit(500m);
        decimal remaining = service.Remaining(spent);
        Assert.Equal(expected, remaining);
    }

    [Theory]
    [InlineData(501)]
    [InlineData(750)]
    [InlineData(1000)]
    public void Evaluate_ReturnsOverBudget_WhenSpentExceedsLimit(decimal spent)
    {
        var service = new BudgetService();
        service.SetMonthlyLimit(500m);
        BudgetStatus status = service.Evaluate(spent);
        Assert.Equal(BudgetStatus.OverBudget, status);
    }

    [Fact]
    public void Remaining_CanReturnNegative_WhenOverBudget()
    {
        var service = new BudgetService();
        service.SetMonthlyLimit(500m);
        decimal remaining = service.Remaining(650m);
        Assert.Equal(-150m, remaining);
    }
}