using FluentAssertions;
using Hisabwala.Application.Shared;
using Xunit;

namespace Hisabwala.Application.Tests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("hello", "Hello")]
    [InlineData("HELLO", "Hello")]
    [InlineData("HeLLo", "Hello")]
    [InlineData("h", "H")]
    [InlineData("A", "A")]
    [InlineData("test123", "Test123")]
    [InlineData("TEST", "Test")]
    public void FirstCharToUpper_WithValidString_ShouldCapitalizeFirstCharAndLowercaseRest(string input, string expected)
    {
        // Act
        var result = input.FirstCharToUpper();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FirstCharToUpper_WithNullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string? input = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => input!.FirstCharToUpper());
        exception.ParamName.Should().Be("input");
    }

    [Fact]
    public void FirstCharToUpper_WithEmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        var input = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => input.FirstCharToUpper());
        exception.ParamName.Should().Be("input");
        exception.Message.Should().Contain("cannot be empty");
    }

    [Theory]
    [InlineData("  hello  ", "Hello")]
    [InlineData("  WORLD  ", "World")]
    [InlineData(" test ", "Test")]
    public void FirstCharToUpper_WithWhitespace_ShouldTrimAndCapitalize(string input, string expected)
    {
        // Act
        var result = input.FirstCharToUpper();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("multiple words here", "Multiple words here")]
    [InlineData("MULTIPLE WORDS HERE", "Multiple words here")]
    [InlineData("Multiple Words Here", "Multiple words here")]
    public void FirstCharToUpper_WithMultipleWords_ShouldOnlyCapitalizeFirst(string input, string expected)
    {
        // Act
        var result = input.FirstCharToUpper();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123abc", "123abc")]
    [InlineData("!hello", "!hello")]
    [InlineData("@test", "@test")]
    public void FirstCharToUpper_WithSpecialCharactersAtStart_ShouldHandleCorrectly(string input, string expected)
    {
        // Act
        var result = input.FirstCharToUpper();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("café", "Café")]
    [InlineData("naïve", "Naïve")]
    [InlineData("über", "Über")]
    public void FirstCharToUpper_WithUnicodeCharacters_ShouldHandleCorrectly(string input, string expected)
    {
        // Act
        var result = input.FirstCharToUpper();

        // Assert
        result.Should().Be(expected);
    }
}