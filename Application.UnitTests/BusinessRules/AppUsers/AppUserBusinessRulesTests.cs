using MiniETrade.Application.BusinessRules.AppUsers;
using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Domain.Entities.Identity;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.BusinessRules.AppUsers;

public class AppUserBusinessRulesTests
{
    private readonly AppUserBusinessRules _appUserBusinessRules;
    private readonly Mock<IIdentityService> _identityService;

    public AppUserBusinessRulesTests()
    {
        _identityService = new(); 
        _appUserBusinessRules = new(_identityService.Object);
    }

    [Theory]
    [InlineData("SomePascalCasePassword", "somepascalcasepassword")]
    [InlineData("", "          ")]
    [InlineData("AgoodPassword ", "AgoodPassword")]
    public void WhenPasswordsDontMatchShouldThrowError(string password, string passwordRepeat)
    {
        Action action = () => AppUserBusinessRules.CheckIfPasswordMatches(password, passwordRepeat);
        action.Should().Throw<BusinessException>().WithMessage(AppMessages.PasswordDoesntMatch);
    }

    [Theory]
    [InlineData("ThistimeI'mveryserious!!", "ThistimeI'mveryserious!!")]
    public void WhenPasswordsDontMatchShouldNotThrowError(string password, string passwordRepeat)
    {
        Action action = () => AppUserBusinessRules.CheckIfPasswordMatches(password, passwordRepeat);
        action.Should().NotThrow<BusinessException>();
    }

    [Fact]
    public void ShouldNotRegisterNewUserIfUsernameIsNotAvailable()
    {
        _identityService.Setup(x => x.FindByUserNameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<AppUser>());
        Func<Task> action = async () => await _appUserBusinessRules.CheckIfUsernameIsAvailable("somenewusername");
        action.Should().ThrowAsync<BusinessException>().WithMessage(AppMessages.UsernameAlreadyRegistered);
    }

    [Fact]
    public void ShouldNotRegisterNewUserIfEmailIsNotAvailable()
    {
        _identityService.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<AppUser>());
        Func<Task> action = async () => await _appUserBusinessRules.CheckIfEmailIsAvailable("somerandomemail@gmail.com");
        action.Should().ThrowAsync<BusinessException>().WithMessage(AppMessages.EmailAlreadyRegistered);
    }
}