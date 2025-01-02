namespace DomainAbstractions.Exceptions;
[Serializable]
public class UserUnauthorizedException : Exception
{
    public UserUnauthorizedException(string message) : base(message) { }

    public UserUnauthorizedException(string message, Exception inner) : base(message, inner) { }
}
