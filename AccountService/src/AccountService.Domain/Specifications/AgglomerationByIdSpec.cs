using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class AgglomerationByIdSpec : SingleResultSpecification<Agglomeration>
{
    public AgglomerationByIdSpec(Guid id)
    {
        Query
            .Where(a => a.Id == id);
    }
}
