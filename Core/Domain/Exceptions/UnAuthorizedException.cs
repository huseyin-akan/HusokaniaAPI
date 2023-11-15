
namespace Domain.Exceptions;

public class UnAuthorizedException : Exception
{
    public UnAuthorizedException(string message) : base(message)
    {
    }

    public UnAuthorizedException() : this("Unauthorized action!!!")
    {
    }
}