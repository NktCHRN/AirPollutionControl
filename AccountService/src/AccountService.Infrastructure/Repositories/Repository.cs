using AccountService.Domain.Abstractions;
using AccountService.Infrastructure.Persistence;
using Ardalis.Specification.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories;
public sealed class Repository<T> : RepositoryBase<T>, IRepository<T> where T : class
{
    public Repository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
