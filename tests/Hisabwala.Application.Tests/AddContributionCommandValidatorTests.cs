using FluentAssertions;
using Hisabwala.Application.Features.Party.AddContribution;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Entities;
using Moq;
using Xunit;

namespace Hisabwala.Application.Tests;

public class AddContributionCommandValidatorTests
{
    private readonly Mock<IPartyRepository> _mockPartyRepository;
    private readonly AddContributionCommandValidator _validator;
    private readonly CancellationToken _cancellationToken;

    public AddContributionCommandValidatorTests()
    {
        _mockPartyRepository = new Mock<IPartyRepository>();
        _validator = new AddContributionCommandValidator(_mockPartyRepository.Object);
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task ValidateAsync_WithValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var party = CreateTestParty();
        party.UpdateTags(new List<string> { "Food", "Accommodation" });

        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        };

        _mockPartyRepository.Setup(x => x.PartyExistsAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(true);
        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WithInvalidPartyCode_ShouldReturnFailure()
    {
        // Arrange
        var command = new AddContributionCommand
        {
            PartyCode = "invalid",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        };

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party code must be 9 characters.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateAsync_WithNullOrEmptyName_ShouldReturnFailure(string? name)
    {
        // Arrange
        var party = CreateTestParty();
        party.UpdateTags(new List<string> { "Food" });

        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = name!,
            Tags = new List<string> { "Food" }
        };

        _mockPartyRepository.Setup(x => x.PartyExistsAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(true);
        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Name is required.");
    }

    [Fact]
    public async Task ValidateAsync_WithEmptyTagsList_ShouldReturnFailure()
    {
        // Arrange
        var party = CreateTestParty();
        party.UpdateTags(new List<string> { "Food" });

        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string>()
        };

        _mockPartyRepository.Setup(x => x.PartyExistsAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(true);
        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Atleast one tag is required to calculate contribution.");
    }

    [Fact]
    public async Task ValidateAsync_WithInvalidTag_ShouldReturnFailure()
    {
        // Arrange
        var party = CreateTestParty();
        party.UpdateTags(new List<string> { "Food", "Accommodation" });

        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string> { "InvalidTag" }
        };

        _mockPartyRepository.Setup(x => x.PartyExistsAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(true);
        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid tag used.");
    }

    [Fact]
    public async Task ValidateAsync_WithMultipleValidTags_ShouldReturnSuccess()
    {
        // Arrange
        var party = CreateTestParty();
        party.UpdateTags(new List<string> { "Food", "Accommodation", "Transport" });

        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string> { "Food", "Accommodation" }
        };

        _mockPartyRepository.Setup(x => x.PartyExistsAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(true);
        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateAsync_WithMixedValidAndInvalidTags_ShouldReturnFailure()
    {
        // Arrange
        var party = CreateTestParty();
        party.UpdateTags(new List<string> { "Food", "Accommodation" });

        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string> { "Food", "InvalidTag" }
        };

        _mockPartyRepository.Setup(x => x.PartyExistsAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(true);
        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Invalid tag used.");
    }

    [Fact]
    public async Task ValidateAsync_WhenPartyDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        };

        _mockPartyRepository.Setup(x => x.PartyExistsAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party with the given code does not exist.");
    }

    [Theory]
    [InlineData("Food")]
    [InlineData("food")]
    [InlineData("FOOD")]
    public async Task ValidateAsync_WithCaseInsensitiveTag_ShouldValidateCorrectly(string tag)
    {
        // Arrange
        var party = CreateTestParty();
        party.UpdateTags(new List<string> { "Food", "Accommodation" });

        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string> { tag }
        };

        _mockPartyRepository.Setup(x => x.PartyExistsAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(true);
        _mockPartyRepository.Setup(x => x.GetPartyAsync(command.PartyCode, _cancellationToken))
            .ReturnsAsync(party);

        // Act
        var result = await _validator.ValidateAsync(command, _cancellationToken);

        // Assert
        result.Success.Should().BeTrue();
    }

    private Party CreateTestParty()
    {
        return new Party
        {
            Id = "test-id",
            PartyCode = "abc-def12",
            PartyName = "Test Party",
            CreatedDateTime = DateTime.UtcNow
        };
    }
}