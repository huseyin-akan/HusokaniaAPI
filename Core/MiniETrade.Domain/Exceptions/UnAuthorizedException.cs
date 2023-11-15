using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Domain.Exceptions;

public class UnAuthorizedException : Exception
{
    public UnAuthorizedException(string message) : base(message)
    {
    }

    public UnAuthorizedException() : this("Unauthorized action!!!")
    {
    }
}