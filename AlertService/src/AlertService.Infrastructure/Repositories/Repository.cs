using AlertService.Domain.Abstractions;
using AlertService.Infrastructure.Persistence;
using Ardalis.Specification.EntityFrameworkCore;

namespace AlertService.Infrastructure.Repositories;
public sealed class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
