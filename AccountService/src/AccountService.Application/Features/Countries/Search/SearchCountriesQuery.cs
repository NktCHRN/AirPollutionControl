using Application.Dto;
using Application.Queries;
using MediatR;

namespace AccountService.Application.Features.Countries.Search;
public sealed record SearchCountriesQuery(int PerPage, int Page, string? SearchText) : PagedSearchQuery(PerPage, Page, SearchText), IRequest<PagedDto<CountryDto>>;
