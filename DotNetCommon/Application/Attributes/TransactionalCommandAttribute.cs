namespace Application.Attributes;

// Maybe, replace with Command/Query interfaces in the future? Commands should be always transactional.
[AttributeUsage(AttributeTargets.Class)]
public sealed class TransactionalCommandAttribute : Attribute
{
}
