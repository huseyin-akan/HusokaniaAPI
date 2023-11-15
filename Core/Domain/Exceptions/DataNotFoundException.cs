using Domain.Messages;

namespace Domain.Exceptions;

public class DataNotFoundException : Exception
{
    public DataNotFoundException(string message) : base(message)
    {
    }

    public DataNotFoundException() : this(AppMessages.DataNotFound)
    {}
}