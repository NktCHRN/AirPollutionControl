namespace DomainAbstractions.Exceptions;
[Serializable]
public sealed class ForbiddenForUserException : Exception
{
    public ForbiddenForUserException(string message) : base(message) { }

    public ForbiddenForUserException(string message, Exception inner) : base(message, inner) { }
}
