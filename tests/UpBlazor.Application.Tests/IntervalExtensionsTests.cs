using FluentAssertions;
using UpBlazor.Domain.Helpers;
using UpBlazor.Domain.Models.Enums;

namespace UpBlazor.Application.Tests;

public class IntervalExtensionsTests
{
    [Fact]
    public void FindFirstCycle_WithSearchLandingOnCycle_ReturnsSearchDate()
    {
        // Arrange
        var repeatingSince = new DateTime(2022, 1, 1);
        var searchStart = new DateTime(2022, 1, 22);
        var interval = Interval.Weeks;
        var intervalUnits = 1;

        // Act
        var firstSearchResult = repeatingSince.FindFirstCycle(interval, intervalUnits, searchStart);

        // Assert
        firstSearchResult.Should().Be(searchStart);
    }
    
    [Fact]
    public void FindFirstCycle_WithSearchBeforeStart_ReturnsStart()
    {
        // Arrange
        var repeatingSince = new DateTime(2022, 1, 22);
        var searchStart = new DateTime(2022, 1, 1);
        var interval = Interval.Weeks;
        var intervalUnits = 1;

        // Act
        var firstSearchResult = repeatingSince.FindFirstCycle(interval, intervalUnits, searchStart);

        // Assert
        firstSearchResult.Should().Be(repeatingSince);
    }
}