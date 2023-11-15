using MiniETrade.Application.Common.Abstractions.Identity;
using MiniETrade.Domain.Exceptions;
using MiniETrade.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.BusinessRules.AppUsers;

public class AppUserBusinessRules : BaseBusinessRules
{
    private readonly IIdentityService _identityService;

    public AppUserBusinessRules(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public static void CheckIfPasswordMatches(string password, string passwordToCheck)
    {
        if (password != passwordToCheck) throw new BusinessException(AppMessages.PasswordDoesntMatch); 
    }

    public async Task CheckIfUsernameIsAvailable(string userName)
    {
        var userByUserName = await _identityService.FindByUserNameAsync(userName);
        if (userByUserName != null) throw new BusinessException(AppMessages.UsernameAlreadyRegistered);
    }

    public async Task CheckIfEmailIsAvailable(string email)
    {
        var userByEmail = await _identityService.FindByEmailAsync(email);
        if (userByEmail != null) throw new BusinessException(AppMessages.EmailAlreadyRegistered);
    }
}