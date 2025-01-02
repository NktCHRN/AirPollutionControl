using Ardalis.Specification;

namespace AccountService.Domain.Abstractions;
public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
