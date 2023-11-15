namespace Domain.Exceptions;

public class UserCreateFailedException :Exception
{
    public UserCreateFailedException(string message) : base(message)
    {
    }

    public UserCreateFailedException() : this("User creation failed!!!")
    {
    }
}
