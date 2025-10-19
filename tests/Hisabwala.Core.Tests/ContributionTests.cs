using FluentAssertions;
using Hisabwala.Core.Entities;
using Xunit;

namespace Hisabwala.Core.Tests;

public class ContributionTests
{
    [Fact]
    public void Contribution_Initialization_ShouldHaveEmptyTagsList()
    {
        // Arrange & Act
        var contribution = new Contribution();

        // Assert
        contribution.Tags.Should().NotBeNull();
        contribution.Tags.Should().BeEmpty();
    }

    [Fact]
    public void Contribution_ShouldAllowSettingProperties()
    {
        // Arrange & Act
        var contribution = new Contribution
        {
            Id = "test-id",
            Name = "Alice",
            Tags = new List<string> { "Food", "Accommodation" },
            Amount = 150.50m
        };

        // Assert
        contribution.Id.Should().Be("test-id");
        contribution.Name.Should().Be("Alice");
        contribution.Tags.Should().BeEquivalentTo(new[] { "Food", "Accommodation" });
        contribution.Amount.Should().Be(150.50m);
    }

    [Fact]
    public void Contribution_Tags_ShouldBeMutable()
    {
        // Arrange
        var contribution = new Contribution
        {
            Id = "test-id",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        };

        // Act
        contribution.Tags.Add("Accommodation");
        contribution.Tags.Add("Transport");

        // Assert
        contribution.Tags.Should().HaveCount(3);
        contribution.Tags.Should().BeEquivalentTo(new[] { "Food", "Accommodation", "Transport" });
    }

    [Fact]
    public void Contribution_Amount_ShouldDefaultToZero()
    {
        // Arrange & Act
        var contribution = new Contribution
        {
            Id = "test-id",
            Name = "Alice"
        };

        // Assert
        contribution.Amount.Should().Be(0);
    }

    [Fact]
    public void Contribution_ShouldSupportDecimalAmounts()
    {
        // Arrange & Act
        var contribution = new Contribution
        {
            Id = "test-id",
            Name = "Alice",
            Amount = 123.456789m
        };

        // Assert
        contribution.Amount.Should().Be(123.456789m);
    }
}