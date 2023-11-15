using FluentValidation.TestHelper;
using MiniETrade.Application.Features.AppUsers.Commands.LoginUser;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Features.AppUsers.Commands;

public class LoginUserValidationTests
{
    [Theory]
    [InlineData("", "Password123")]
    [InlineData("       ", "Password123")]
    [InlineData(null, "Password123")]
    public void ShouldThrowErrorWhenUsernameOrEmailIsEmpty(string userNameOrEmail, string password)
    {
        // Arrange
        var validator = new LoginUserValidator();
        var command = new LoginUserCommand(userNameOrEmail, password);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.UsernameOrEmail)
            .WithErrorMessage(AppMessages.UsernameOrEmailEmpty);
    }

    [Theory]
    [InlineData("someUsername", "")]
    [InlineData("someUsername", "  ")]
    [InlineData("someUsername", null)]
    [InlineData("someUsername", "  asd  ")]
    [InlineData("someUsername", "12345")]
    public void ShouldThrowErrorWhenPasswordEmpty(string userNameOrEmail, string password)
    {
        // Arrange
        var validator = new LoginUserValidator();
        var command = new LoginUserCommand(userNameOrEmail, password);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.UsernameOrEmail)
            .WithErrorMessage(AppMessages.PasswordEmpty);
    }

    [Theory]
    [InlineData("someUsername", "  asd  ")]
    [InlineData("someUsername", "12345")]
    public void ShouldThrowErrorWhenPasswordIsTooShort(string userNameOrEmail, string password)
    {
        // Arrange
        var validator = new LoginUserValidator();
        var command = new LoginUserCommand(userNameOrEmail, password);

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.UsernameOrEmail)
            .WithErrorMessage(AppMessages.PasswordMinLength);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenValidCommand()
    {
        // Arrange
        var validator = new LoginUserValidator();
        var command = new LoginUserCommand("user@example.com", "Password123");

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}