using FluentAssertions;
using Hisabwala.Core.Entities;
using Xunit;

namespace Hisabwala.Core.Tests;

public class PartyTests
{
    [Fact]
    public void UpdateTags_WithNewTags_ShouldReplaceTags()
    {
        // Arrange
        var party = new Party
        {
            Id = "test-id",
            PartyCode = "test-code",
            PartyName = "Test Party"
        };

        var newTags = new List<string> { "Food", "Accommodation", "Transport" };

        // Act
        party.UpdateTags(newTags);

        // Assert
        party.Tags.Should().BeEquivalentTo(newTags);
    }

    [Fact]
    public void UpdateTags_WithEmptyList_ShouldClearTags()
    {
        // Arrange
        var party = new Party
        {
            Id = "test-id",
            PartyCode = "test-code",
            PartyName = "Test Party"
        };
        party.UpdateTags(new List<string> { "Food", "Accommodation" });

        // Act
        party.UpdateTags(new List<string>());

        // Assert
        party.Tags.Should().BeEmpty();
    }

    [Fact]
    public void UpdateTags_ShouldNotAffectOtherProperties()
    {
        // Arrange
        var party = new Party
        {
            Id = "test-id",
            PartyCode = "test-code",
            PartyName = "Test Party",
            CreatedDateTime = new DateTime(2024, 1, 1)
        };
        party.Expenses.Add(new Expense { Id = "1", Name = "Test", Amount = 100, PaidBy = "Alice", Tag = "Food" });
        party.Contributions.Add(new Contribution { Id = "1", Name = "Alice", Tags = new List<string> { "Food" } });

        // Act
        party.UpdateTags(new List<string> { "Food", "Transport" });

        // Assert
        party.Id.Should().Be("test-id");
        party.PartyCode.Should().Be("test-code");
        party.PartyName.Should().Be("Test Party");
        party.CreatedDateTime.Should().Be(new DateTime(2024, 1, 1));
        party.Expenses.Should().HaveCount(1);
        party.Contributions.Should().HaveCount(1);
    }

    [Fact]
    public void UpdateTags_WithDuplicateTags_ShouldKeepDuplicates()
    {
        // Arrange
        var party = new Party
        {
            Id = "test-id",
            PartyCode = "test-code",
            PartyName = "Test Party"
        };

        var newTags = new List<string> { "Food", "Food", "Accommodation" };

        // Act
        party.UpdateTags(newTags);

        // Assert
        party.Tags.Should().HaveCount(3);
        party.Tags.Should().BeEquivalentTo(newTags);
    }

    [Fact]
    public void Party_Initialization_ShouldHaveEmptyCollections()
    {
        // Arrange & Act
        var party = new Party
        {
            Id = "test-id",
            PartyCode = "test-code",
            PartyName = "Test Party"
        };

        // Assert
        party.Tags.Should().NotBeNull();
        party.Tags.Should().BeEmpty();
        party.Expenses.Should().NotBeNull();
        party.Expenses.Should().BeEmpty();
        party.Contributions.Should().NotBeNull();
        party.Contributions.Should().BeEmpty();
    }

    [Fact]
    public void Party_CreatedDateTime_ShouldBeUtc()
    {
        // Arrange & Act
        var party = new Party
        {
            Id = "test-id",
            PartyCode = "test-code",
            PartyName = "Test Party"
        };

        // Assert
        party.CreatedDateTime.Kind.Should().Be(DateTimeKind.Utc);
    }
}