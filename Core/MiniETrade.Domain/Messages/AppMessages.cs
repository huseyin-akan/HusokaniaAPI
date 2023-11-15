using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Messages;

public static class AppMessages
{
    //Auth Messages
    public static string PasswordDoesntMatch = "PasswordDoesntMatch";
    public static string RoleAlreadyExists = "RoleAlreadyExists";
    public static string RoleDoesntExist = "RoleDoesntExist";
    public static string UserNotFound = "UserNotFound";
    public static string UserCreateFailed = "UserCreateFailed";
    public static string UsernameAlreadyRegistered = "UsernameAlreadyRegistered";
    public static string EmailAlreadyRegistered = "EmailAlreadyRegistered";
    public static string LoginUnsuccessful = "LoginUnsuccessful";
    public static string UnauthorizedAttempt = "UnauthorizedAttempt";

    //Validation Messages
    public static string InvalidEmailAddress = "InvalidEmailAddress";
    public static string UsernameOrEmailEmpty = "UsernameOrEmailEmpty";
    public static string FieldCannotBeEmpty = "FieldCannotBeEmpty";
    public static string PasswordEmpty = "PasswordEmpty";
    public static string PasswordMinLength = "PasswordMinLength";

    //Product Messages
    public static string ProductNotAvailable = "ProductNotAvailable";
    public static string DataNotFound = "DataNotFound";

    //Error Messages
    public static string UnexpectedError = "UnexpectedError";
}