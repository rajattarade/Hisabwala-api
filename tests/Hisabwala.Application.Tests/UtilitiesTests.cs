using FluentAssertions;
using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Entities;
using Moq;
using Xunit;

namespace Hisabwala.Application.Tests;

public class UtilitiesTests
{
    private readonly Mock<IPartyRepository> _mockPartyRepository;
    private readonly CancellationToken _cancellationToken;

    public UtilitiesTests()
    {
        _mockPartyRepository = new Mock<IPartyRepository>();
        _cancellationToken = CancellationToken.None;
    }

    #region ValidatePartyCode Tests

    [Fact]
    public async Task ValidatePartyCode_WithNullPartyCode_ShouldReturnFailure()
    {
        // Arrange
        string? partyCode = null;

        // Act
        var result = await Utilities.ValidatePartyCode(partyCode!, _cancellationToken, _mockPartyRepository.Object);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party code is required.");
    }

    [Fact]
    public async Task ValidatePartyCode_WithEmptyPartyCode_ShouldReturnFailure()
    {
        // Arrange
        var partyCode = string.Empty;

        // Act
        var result = await Utilities.ValidatePartyCode(partyCode, _cancellationToken, _mockPartyRepository.Object);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party code is required.");
    }

    [Fact]
    public async Task ValidatePartyCode_WithWhitespacePartyCode_ShouldReturnFailure()
    {
        // Arrange
        var partyCode = "   ";

        // Act
        var result = await Utilities.ValidatePartyCode(partyCode, _cancellationToken, _mockPartyRepository.Object);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party code is required.");
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12345678")]
    [InlineData("1234567890")]
    [InlineData("ab")]
    public async Task ValidatePartyCode_WithInvalidLength_ShouldReturnFailure(string partyCode)
    {
        // Act
        var result = await Utilities.ValidatePartyCode(partyCode, _cancellationToken, _mockPartyRepository.Object);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party code must be 9 characters.");
    }

    [Theory]
    [InlineData("abc def12")]
    [InlineData("abc@def12")]
    [InlineData("abc_def12")]
    [InlineData("abc.def12")]
    [InlineData("abc/def12")]
    public async Task ValidatePartyCode_WithInvalidCharacters_ShouldReturnFailure(string partyCode)
    {
        // Act
        var result = await Utilities.ValidatePartyCode(partyCode, _cancellationToken, _mockPartyRepository.Object);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party name must only contain letters, numbers and, hyphen (-).");
    }

    [Fact]
    public async Task ValidatePartyCode_WhenPartyDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var partyCode = "abc-def12";
        _mockPartyRepository.Setup(x => x.PartyExistsAsync(partyCode, _cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await Utilities.ValidatePartyCode(partyCode, _cancellationToken, _mockPartyRepository.Object);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party with the given code does not exist.");
    }

    [Theory]
    [InlineData("abc-def12")]
    [InlineData("ABCDEF123")]
    [InlineData("abc123-45")]
    [InlineData("123456789")]
    [InlineData("abc-DEF-1")]
    public async Task ValidatePartyCode_WithValidPartyCode_ShouldReturnSuccess(string partyCode)
    {
        // Arrange
        _mockPartyRepository.Setup(x => x.PartyExistsAsync(partyCode, _cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await Utilities.ValidatePartyCode(partyCode, _cancellationToken, _mockPartyRepository.Object);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();
    }

    #endregion

    #region UpdateContributions Tests

    [Fact]
    public void UpdateContributions_WithNoExpensesOrContributions_ShouldNotThrow()
    {
        // Arrange
        var party = CreateTestParty();

        // Act
        Action act = () => party.UpdateContributions();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void UpdateContributions_WithSingleExpenseAndSingleContributor_ShouldCalculateCorrectly()
    {
        // Arrange
        var party = CreateTestParty();
        party.Expenses.Add(new Expense
        {
            Id = "1",
            Name = "Lunch",
            Amount = 100,
            PaidBy = "Alice",
            Tag = "Food"
        });
        party.Contributions.Add(new Contribution
        {
            Id = "1",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        });

        // Act
        party.UpdateContributions();

        // Assert
        party.Contributions[0].Amount.Should().Be(100);
    }

    [Fact]
    public void UpdateContributions_WithMultipleExpensesSameTag_ShouldSumCorrectly()
    {
        // Arrange
        var party = CreateTestParty();
        party.Expenses.Add(new Expense { Id = "1", Name = "Lunch", Amount = 100, PaidBy = "Alice", Tag = "Food" });
        party.Expenses.Add(new Expense { Id = "2", Name = "Dinner", Amount = 200, PaidBy = "Bob", Tag = "Food" });
        
        party.Contributions.Add(new Contribution { Id = "1", Name = "Alice", Tags = new List<string> { "Food" } });
        party.Contributions.Add(new Contribution { Id = "2", Name = "Bob", Tags = new List<string> { "Food" } });

        // Act
        party.UpdateContributions();

        // Assert
        party.Contributions[0].Amount.Should().Be(150); // (100 + 200) / 2
        party.Contributions[1].Amount.Should().Be(150); // (100 + 200) / 2
    }

    [Fact]
    public void UpdateContributions_WithDifferentTags_ShouldCalculateSeparately()
    {
        // Arrange
        var party = CreateTestParty();
        party.Expenses.Add(new Expense { Id = "1", Name = "Lunch", Amount = 100, PaidBy = "Alice", Tag = "Food" });
        party.Expenses.Add(new Expense { Id = "2", Name = "Hotel", Amount = 300, PaidBy = "Bob", Tag = "Accommodation" });
        
        party.Contributions.Add(new Contribution { Id = "1", Name = "Alice", Tags = new List<string> { "Food" } });
        party.Contributions.Add(new Contribution { Id = "2", Name = "Bob", Tags = new List<string> { "Accommodation" } });

        // Act
        party.UpdateContributions();

        // Assert
        party.Contributions[0].Amount.Should().Be(100);
        party.Contributions[1].Amount.Should().Be(300);
    }

    [Fact]
    public void UpdateContributions_WithMultipleTags_ShouldCalculateForEachTag()
    {
        // Arrange
        var party = CreateTestParty();
        party.Expenses.Add(new Expense { Id = "1", Name = "Lunch", Amount = 100, PaidBy = "Alice", Tag = "Food" });
        party.Expenses.Add(new Expense { Id = "2", Name = "Hotel", Amount = 300, PaidBy = "Bob", Tag = "Accommodation" });
        
        party.Contributions.Add(new Contribution 
        { 
            Id = "1", 
            Name = "Alice", 
            Tags = new List<string> { "Food", "Accommodation" } 
        });
        party.Contributions.Add(new Contribution 
        { 
            Id = "2", 
            Name = "Bob", 
            Tags = new List<string> { "Food", "Accommodation" } 
        });

        // Act
        party.UpdateContributions();

        // Assert
        party.Contributions[0].Amount.Should().Be(200); // 50 (food) + 150 (accommodation)
        party.Contributions[1].Amount.Should().Be(200); // 50 (food) + 150 (accommodation)
    }

    [Fact]
    public void UpdateContributions_WithRounding_ShouldRoundCorrectly()
    {
        // Arrange
        var party = CreateTestParty();
        party.Expenses.Add(new Expense { Id = "1", Name = "Lunch", Amount = 100, PaidBy = "Alice", Tag = "Food" });
        
        party.Contributions.Add(new Contribution { Id = "1", Name = "Alice", Tags = new List<string> { "Food" } });
        party.Contributions.Add(new Contribution { Id = "2", Name = "Bob", Tags = new List<string> { "Food" } });
        party.Contributions.Add(new Contribution { Id = "3", Name = "Charlie", Tags = new List<string> { "Food" } });

        // Act
        party.UpdateContributions();

        // Assert
        // 100 / 3 = 33.33..., should round to 33
        party.Contributions.Sum(c => c.Amount).Should().Be(99); // 33 * 3
    }

    [Fact]
    public void UpdateContributions_WithDuplicateTagsInContribution_ShouldCountOnce()
    {
        // Arrange
        var party = CreateTestParty();
        party.Expenses.Add(new Expense { Id = "1", Name = "Lunch", Amount = 100, PaidBy = "Alice", Tag = "Food" });
        
        party.Contributions.Add(new Contribution 
        { 
            Id = "1", 
            Name = "Alice", 
            Tags = new List<string> { "Food", "Food", "Food" } // Duplicate tags
        });

        // Act
        party.UpdateContributions();

        // Assert
        party.Contributions[0].Amount.Should().Be(100); // Should count only once
    }

    [Fact]
    public void UpdateContributions_WithComplexScenario_ShouldCalculateCorrectly()
    {
        // Arrange
        var party = CreateTestParty();
        
        // Add multiple expenses across different tags
        party.Expenses.Add(new Expense { Id = "1", Name = "Breakfast", Amount = 60, PaidBy = "Alice", Tag = "Food" });
        party.Expenses.Add(new Expense { Id = "2", Name = "Lunch", Amount = 120, PaidBy = "Bob", Tag = "Food" });
        party.Expenses.Add(new Expense { Id = "3", Name = "Hotel", Amount = 300, PaidBy = "Charlie", Tag = "Accommodation" });
        party.Expenses.Add(new Expense { Id = "4", Name = "Taxi", Amount = 50, PaidBy = "Alice", Tag = "Transport" });
        
        // Add contributions with different tag combinations
        party.Contributions.Add(new Contribution 
        { 
            Id = "1", 
            Name = "Alice", 
            Tags = new List<string> { "Food", "Accommodation", "Transport" } 
        });
        party.Contributions.Add(new Contribution 
        { 
            Id = "2", 
            Name = "Bob", 
            Tags = new List<string> { "Food", "Accommodation" } 
        });
        party.Contributions.Add(new Contribution 
        { 
            Id = "3", 
            Name = "Charlie", 
            Tags = new List<string> { "Food" } 
        });

        // Act
        party.UpdateContributions();

        // Assert
        // Food: (60 + 120) / 3 = 60 each
        // Accommodation: 300 / 2 = 150 each (Alice and Bob)
        // Transport: 50 / 1 = 50 (Alice only)
        party.Contributions.First(c => c.Name == "Alice").Amount.Should().Be(260); // 60 + 150 + 50
        party.Contributions.First(c => c.Name == "Bob").Amount.Should().Be(210); // 60 + 150
        party.Contributions.First(c => c.Name == "Charlie").Amount.Should().Be(60); // 60
    }

    [Fact]
    public void UpdateContributions_WithZeroAmountExpense_ShouldHandleCorrectly()
    {
        // Arrange
        var party = CreateTestParty();
        party.Expenses.Add(new Expense { Id = "1", Name = "Free", Amount = 0, PaidBy = "Alice", Tag = "Food" });
        
        party.Contributions.Add(new Contribution { Id = "1", Name = "Alice", Tags = new List<string> { "Food" } });

        // Act
        party.UpdateContributions();

        // Assert
        party.Contributions[0].Amount.Should().Be(0);
    }

    [Fact]
    public void UpdateContributions_ResetsAmountsBeforeCalculation()
    {
        // Arrange
        var party = CreateTestParty();
        party.Expenses.Add(new Expense { Id = "1", Name = "Lunch", Amount = 100, PaidBy = "Alice", Tag = "Food" });
        
        party.Contributions.Add(new Contribution 
        { 
            Id = "1", 
            Name = "Alice", 
            Tags = new List<string> { "Food" },
            Amount = 999 // Pre-existing amount
        });

        // Act
        party.UpdateContributions();

        // Assert
        party.Contributions[0].Amount.Should().Be(100); // Should be recalculated, not added to existing
    }

    #endregion

    private Party CreateTestParty()
    {
        return new Party
        {
            Id = "test-id",
            PartyCode = "test-code",
            PartyName = "Test Party",
            CreatedDateTime = DateTime.UtcNow
        };
    }
}