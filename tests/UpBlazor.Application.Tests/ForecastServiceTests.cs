using FluentAssertions;
using MediatR;
using Moq;
using UpBlazor.Application.Features.RecurringExpenses;
using UpBlazor.Application.Services;
using UpBlazor.Core.Models;
using UpBlazor.Core.Models.Enums;

namespace UpBlazor.Application.Tests;

public class ForecastServiceTests
{
    [Fact]
    public async Task GetRecurringExpenseCyclesInRangeAsync_WithWeeklyExpense_WithMonthDuration_ReturnsSeveralTimes()
    {
        // Arrange
        var recurringExpenses = new List<RecurringExpense>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Rent",
                Money = new Money
                {
                    Exact = 100
                },
                Interval = Interval.Weeks,
                IntervalUnits = 1,
                StartDate = new DateTime(2022, 1, 1)
            }
        };
            
        
        var mockMediator = new Mock<IMediator>();
        mockMediator
            .Setup(x => x.Send(It.IsAny<GetRecurringExpensesQuery>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult<IReadOnlyList<RecurringExpense>>(recurringExpenses));
        
        var forecastService = new ForecastService(mockMediator.Object);

        var searchStart = new DateTime(2022, 2, 1);
        var searchEnd = new DateTime(2022, 3, 1);

        // Act
        var cyclesInRange = await forecastService.GetRecurringExpenseCyclesInRangeAsync(searchStart, searchEnd);

        // Assert
        cyclesInRange.Keys.Should().HaveCount(1).And.ContainSingle(x => x == recurringExpenses.First().Id);
        cyclesInRange.Values.Should().HaveCount(1);
        
        var cycles = cyclesInRange.Values.First();
        cycles.Should().HaveCount(4);
    }
}