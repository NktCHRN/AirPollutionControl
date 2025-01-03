using Ardalis.Specification;

namespace AlertService.Domain.Abstractions;
public interface IRepository<T> : IRepositoryBase<T> where T : class
{
}
