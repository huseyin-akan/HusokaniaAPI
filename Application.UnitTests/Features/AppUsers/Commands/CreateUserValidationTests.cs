using FluentValidation.TestHelper;
using MiniETrade.Application.Features.AppUsers.Commands.CreateUser;
using MiniETrade.Domain.Messages;
using MiniETrade.Infrastructure.Helpers.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Features.AppUsers.Commands;

public class CreateUserValidationTests
{
    [Fact]
    public void ShouldNotValidateEmailFieldCorrectly()
    {
        // Arrange
        var validationHelper = new FluentValidationTestHelper<CreateAppUserValidator, CreateUserCommand>();
        CreateUserCommand model = new();

        //Act & Arrange
        validationHelper.TestEmailFieldForSuccessCases(a => a.Email, model);
        validationHelper.TestEmailFieldForErrorCases(a => a.Email, model, AppMessages.InvalidEmailAddress);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("    ")]
    public void ShouldThrowErrorWhenPasswordIsEmpty(string password)
    {
        // Arrange
        var validator = new CreateAppUserValidator();
        var command = new CreateUserCommand
        {
            Password = password, 
            Email = "user@example.com"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Password)
            .WithErrorMessage(AppMessages.PasswordEmpty);
    }

    [Theory]
    [InlineData("  123 ")]
    [InlineData("12345")]
    public void ShouldThrowErrorWhenPasswordIsTooShort(string password)
    {
        // Arrange
        var validator = new CreateAppUserValidator();
        var command = new CreateUserCommand
        {
            Password = password,
            Email = "user@example.com"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(u => u.Password)
            .WithErrorMessage(AppMessages.PasswordMinLength);
    }

    [Fact]
    public void ShouldNotHaveErrorWhenValidCommand()
    {
        // Arrange
        var validator = new CreateAppUserValidator();
        var command = new CreateUserCommand
        {
            Password = "Password123", 
            Email = "user@example.com"
        };

        // Act
        var result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}