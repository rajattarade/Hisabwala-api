using FluentAssertions;
using Hisabwala.Application.Features.Party.AddContribution;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Entities;
using Moq;
using Xunit;

namespace Hisabwala.Application.Tests;

public class AddContributionCommandHandlerTests
{
    private readonly Mock<IPartyRepository> _mockPartyRepository;
    private readonly AddContributionCommandHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public AddContributionCommandHandlerTests()
    {
        _mockPartyRepository = new Mock<IPartyRepository>();
        _handler = new AddContributionCommandHandler(_mockPartyRepository.Object);
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task Handle_WithNewContributor_ShouldAddContributorToParty()
    {
        // Arrange
        var party = CreateTestParty();
        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "alice",
            Tags = new List<string> { "food" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().NotBeNullOrEmpty();
        
        party.Contributions.Should().HaveCount(1);
        party.Contributions[0].Name.Should().Be("Alice");
        party.Contributions[0].Tags.Should().BeEquivalentTo(new[] { "Food" });
        
        _mockPartyRepository.Verify(x => x.UpdatePartyAsync(party, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingContributor_ShouldAddTagsToExistingContributor()
    {
        // Arrange
        var party = CreateTestParty();
        party.Contributions.Add(new Contribution
        {
            Id = "existing-id",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        });

        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "alice",
            Tags = new List<string> { "accommodation" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        party.Contributions.Should().HaveCount(1);
        party.Contributions[0].Name.Should().Be("Alice");
        party.Contributions[0].Tags.Should().BeEquivalentTo(new[] { "Food", "Accommodation" });
        
        _mockPartyRepository.Verify(x => x.UpdatePartyAsync(party, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCapitalizeNameAndTags()
    {
        // Arrange
        var party = CreateTestParty();
        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "alice",
            Tags = new List<string> { "food", "accommodation", "transport" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        party.Contributions[0].Name.Should().Be("Alice");
        party.Contributions[0].Tags.Should().BeEquivalentTo(new[] { "Food", "Accommodation", "Transport" });
    }

    [Fact]
    public async Task Handle_WithMultipleTags_ShouldAddAllTags()
    {
        // Arrange
        var party = CreateTestParty();
        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "bob",
            Tags = new List<string> { "food", "accommodation", "transport" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        party.Contributions[0].Tags.Should().HaveCount(3);
        party.Contributions[0].Tags.Should().BeEquivalentTo(new[] { "Food", "Accommodation", "Transport" });
    }

    [Fact]
    public async Task Handle_ShouldUpdateContributions()
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

        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "alice",
            Tags = new List<string> { "food" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        party.Contributions[0].Amount.Should().Be(100);
    }

    [Fact]
    public async Task Handle_WithExistingContributorAndExistingTags_ShouldAppendNewTags()
    {
        // Arrange
        var party = CreateTestParty();
        party.Contributions.Add(new Contribution
        {
            Id = "existing-id",
            Name = "Alice",
            Tags = new List<string> { "Food", "Transport" }
        });

        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "alice",
            Tags = new List<string> { "accommodation", "entertainment" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        party.Contributions.Should().HaveCount(1);
        party.Contributions[0].Tags.Should().BeEquivalentTo(new[] { "Food", "Transport", "Accommodation", "Entertainment" });
    }

    [Fact]
    public async Task Handle_WithCaseInsensitiveName_ShouldMatchExistingContributor()
    {
        // Arrange
        var party = CreateTestParty();
        party.Contributions.Add(new Contribution
        {
            Id = "existing-id",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        });

        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "alice", // lowercase
            Tags = new List<string> { "accommodation" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        party.Contributions.Should().HaveCount(1); // Should not create duplicate
        party.Contributions[0].Name.Should().Be("Alice");
    }

    [Fact]
    public async Task Handle_WithMultipleExistingContributors_ShouldAddCorrectly()
    {
        // Arrange
        var party = CreateTestParty();
        party.Contributions.Add(new Contribution
        {
            Id = "1",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        });
        party.Contributions.Add(new Contribution
        {
            Id = "2",
            Name = "Bob",
            Tags = new List<string> { "Food" }
        });

        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "charlie",
            Tags = new List<string> { "food" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        party.Contributions.Should().HaveCount(3);
        party.Contributions[2].Name.Should().Be("Charlie");
    }

    [Fact]
    public async Task Handle_ShouldGenerateUniqueId()
    {
        // Arrange
        var party = CreateTestParty();
        var command = new AddContributionCommand
        {
            PartyCode = "test-code",
            Name = "alice",
            Tags = new List<string> { "food" }
        };

        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        result.Data!.Id.Should().NotBeNullOrEmpty();
        party.Contributions[0].Id.Should().Be(result.Data.Id);
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