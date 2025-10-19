using FluentAssertions;
using Hisabwala.Application.Features.Party.AddContribution;
using Hisabwala.Core.Common;
using MediatR;
using Moq;
using Xunit;

namespace Hisabwala.Api.Tests;

public class PartyControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly PartyController _controller;

    public PartyControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new PartyController(_mockMediator.Object);
    }

    [Fact]
    public async Task AddContribution_WithValidCommand_ShouldReturnSuccessResult()
    {
        // Arrange
        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        };

        var expectedResult = Result<AddContributionDTO>.Ok(new AddContributionDTO
        {
            Id = "test-id"
        });

        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AddContribution(command);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be("test-id");
        
        _mockMediator.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddContribution_WithFailureResult_ShouldReturnFailureResult()
    {
        // Arrange
        var command = new AddContributionCommand
        {
            PartyCode = "invalid",
            Name = "Alice",
            Tags = new List<string> { "Food" }
        };

        var expectedResult = Result<AddContributionDTO>.Fail("Party code must be 9 characters.");

        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AddContribution(command);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Error.Should().Be("Party code must be 9 characters.");
        result.Data.Should().BeNull();
        
        _mockMediator.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddContribution_ShouldPassCommandToMediator()
    {
        // Arrange
        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string> { "Food", "Accommodation" }
        };

        var expectedResult = Result<AddContributionDTO>.Ok(new AddContributionDTO { Id = "new-id" });

        _mockMediator.Setup(x => x.Send(It.IsAny<AddContributionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        await _controller.AddContribution(command);

        // Assert
        _mockMediator.Verify(x => x.Send(
            It.Is<AddContributionCommand>(c => 
                c.PartyCode == command.PartyCode &&
                c.Name == command.Name &&
                c.Tags.SequenceEqual(command.Tags)),
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task AddContribution_WithEmptyTags_ShouldStillCallMediator()
    {
        // Arrange
        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Alice",
            Tags = new List<string>()
        };

        var expectedResult = Result<AddContributionDTO>.Fail("Atleast one tag is required to calculate contribution.");

        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AddContribution(command);

        // Assert
        result.Success.Should().BeFalse();
        _mockMediator.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddContribution_ShouldReturnResultFromMediator()
    {
        // Arrange
        var command = new AddContributionCommand
        {
            PartyCode = "abc-def12",
            Name = "Bob",
            Tags = new List<string> { "Transport" }
        };

        var mediatorResult = Result<AddContributionDTO>.Ok(new AddContributionDTO { Id = "generated-id" });

        _mockMediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        // Act
        var result = await _controller.AddContribution(command);

        // Assert
        result.Should().BeSameAs(mediatorResult);
    }
}