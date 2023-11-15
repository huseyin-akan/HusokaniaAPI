using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Exceptions;

public class UserCreateFailedException :Exception
{
    public UserCreateFailedException(string message) : base(message)
    {
    }

    public UserCreateFailedException() : this("User creation failed!!!")
    {
    }
}
