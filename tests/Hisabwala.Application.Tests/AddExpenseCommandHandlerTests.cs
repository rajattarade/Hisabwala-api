using FluentAssertions;
using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Entities;
using Moq;
using Xunit;

namespace Hisabwala.Application.Tests;

public class AddExpenseCommandHandlerTests
{
    private readonly Mock<IPartyRepository> _mockPartyRepository;
    private readonly AddExpenseCommandHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public AddExpenseCommandHandlerTests()
    {
        _mockPartyRepository = new Mock<IPartyRepository>();
        _handler = new AddExpenseCommandHandler(_mockPartyRepository.Object);
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task Handle_ShouldCapitalizeNamePaidByAndTag()
    {
        // Arrange
        var party = CreateTestParty();
        var command = new AddExpenseCommand
        {
            PartyCode = "test-code",
            Name = "lunch",
            Amount = 100,
            PaidBy = "alice",
            Tag = "food"
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        var expense = party.Expenses.First();
        expense.Name.Should().Be("Lunch");
        expense.PaidBy.Should().Be("Alice");
        expense.Tag.Should().Be("Food");
    }

    [Fact]
    public async Task Handle_ShouldUpdateTagsInParty()
    {
        // Arrange
        var party = CreateTestParty();
        var command = new AddExpenseCommand
        {
            PartyCode = "test-code",
            Name = "Lunch",
            Amount = 100,
            PaidBy = "Alice",
            Tag = "Food"
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        await _handler.Handle(command, _cancellationToken);

        // Assert
        party.Tags.Should().Contain("Food");
    }

    [Fact]
    public async Task Handle_ShouldAddContributorIfNotExists()
    {
        // Arrange
        var party = CreateTestParty();
        var command = new AddExpenseCommand
        {
            PartyCode = "test-code",
            Name = "Lunch",
            Amount = 100,
            PaidBy = "alice",
            Tag = "food"
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        await _handler.Handle(command, _cancellationToken);

        // Assert
        party.Contributions.Should().HaveCount(1);
        party.Contributions[0].Name.Should().Be("Alice");
        party.Contributions[0].Tags.Should().Contain("Food");
    }

    [Fact]
    public async Task Handle_ShouldAddTagToExistingContributor()
    {
        // Arrange
        var party = CreateTestParty();
        party.Contributions.Add(new Contribution
        {
            Id = "1",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        });

        var command = new AddExpenseCommand
        {
            PartyCode = "test-code",
            Name = "Hotel",
            Amount = 300,
            PaidBy = "Alice",
            Tag = "Accommodation"
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        await _handler.Handle(command, _cancellationToken);

        // Assert
        party.Contributions.Should().HaveCount(1);
        party.Contributions[0].Tags.Should().BeEquivalentTo(new[] { "Food", "Accommodation" });
    }

    [Fact]
    public async Task Handle_ShouldNotDuplicateTagForExistingContributor()
    {
        // Arrange
        var party = CreateTestParty();
        party.Contributions.Add(new Contribution
        {
            Id = "1",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        });

        var command = new AddExpenseCommand
        {
            PartyCode = "test-code",
            Name = "Dinner",
            Amount = 200,
            PaidBy = "Alice",
            Tag = "Food"
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        await _handler.Handle(command, _cancellationToken);

        // Assert
        party.Contributions.Should().HaveCount(1);
        party.Contributions[0].Tags.Should().HaveCount(1);
        party.Contributions[0].Tags.Should().Contain("Food");
    }

    [Fact]
    public async Task Handle_ShouldUpdateContributions()
    {
        // Arrange
        var party = CreateTestParty();
        var command = new AddExpenseCommand
        {
            PartyCode = "test-code",
            Name = "Lunch",
            Amount = 100,
            PaidBy = "Alice",
            Tag = "Food"
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        await _handler.Handle(command, _cancellationToken);

        // Assert
        party.Contributions[0].Amount.Should().Be(100);
    }

    [Fact]
    public async Task Handle_WithMultipleExpenses_ShouldCalculateContributionsCorrectly()
    {
        // Arrange
        var party = CreateTestParty();
        
        // First expense
        var command1 = new AddExpenseCommand
        {
            PartyCode = "test-code",
            Name = "Lunch",
            Amount = 100,
            PaidBy = "Alice",
            Tag = "Food"
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command1.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        await _handler.Handle(command1, _cancellationToken);

        // Second expense
        var command2 = new AddExpenseCommand
        {
            PartyCode = "test-code",
            Name = "Dinner",
            Amount = 200,
            PaidBy = "Bob",
            Tag = "Food"
        };

        await _handler.Handle(command2, _cancellationToken);

        // Assert
        party.Contributions.Should().HaveCount(2);
        party.Contributions[0].Amount.Should().Be(150); // (100 + 200) / 2
        party.Contributions[1].Amount.Should().Be(150); // (100 + 200) / 2
    }

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