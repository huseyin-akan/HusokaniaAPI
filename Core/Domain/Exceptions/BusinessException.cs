
namespace Domain.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }

    public BusinessException() : this("Business rule is violated!!!")
    {
    }
}
