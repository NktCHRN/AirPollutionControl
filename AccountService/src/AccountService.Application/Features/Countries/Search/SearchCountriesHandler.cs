using AccountService.Domain.Abstractions;
using AccountService.Domain.Models;
using AccountService.Domain.Specifications;
using Application.Dto;
using MediatR;

namespace AccountService.Application.Features.Countries.Search;
public sealed class SearchCountriesHandler : IRequestHandler<SearchCountriesQuery, PagedDto<CountryDto>>
{
    private readonly IRepository<Country> countryRepository;

    public SearchCountriesHandler(IRepository<Country> countryRepository)
    {
        this.countryRepository = countryRepository;
    }

    public async Task<PagedDto<CountryDto>> Handle(SearchCountriesQuery request, CancellationToken cancellationToken)
    {
        var totalCount = await countryRepository.CountAsync(new CountriesSearchCountSpec(request.SearchText), cancellationToken);
        var countries = await countryRepository.ListAsync(new CountriesSearchPagedSpec(request.PerPage, request.Page, request.SearchText), cancellationToken);

        return new PagedDto<CountryDto>(
            countries.Select(c => new CountryDto(c.Id, c.Name, c.AdministrationName))
            .ToList(), 
            totalCount);
    }
}
