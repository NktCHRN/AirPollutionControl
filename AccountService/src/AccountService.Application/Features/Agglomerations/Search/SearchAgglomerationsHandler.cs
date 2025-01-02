using AccountService.Domain.Abstractions;
using AccountService.Domain.Models;
using AccountService.Domain.Specifications;
using Application.Dto;
using MediatR;

namespace AccountService.Application.Features.Agglomerations.Search;
public sealed class SearchAgglomerationsHandler : IRequestHandler<SearchAgglomerationsQuery, PagedDto<AgglomerationDto>>
{
    private readonly IRepository<Agglomeration> agglomerationRepository;

    public SearchAgglomerationsHandler(IRepository<Agglomeration> agglomerationRepository)
    {
        this.agglomerationRepository = agglomerationRepository;
    }

    public async Task<PagedDto<AgglomerationDto>> Handle(SearchAgglomerationsQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await agglomerationRepository.CountAsync(new AgglomerationsSearchCountSpec(request.CountryId, request.SearchText), cancellationToken);
        var agglomerations = await agglomerationRepository.ListAsync(new AgglomerationsSearchPagedSpec(request.CountryId, request.PerPage, request.Page, request.SearchText), cancellationToken);

        return new PagedDto<AgglomerationDto>(
            agglomerations.Select(c => new AgglomerationDto(c.Id, c.Name, c.AdministrationName))
            .ToList(),
            totalCount);
    }
}
