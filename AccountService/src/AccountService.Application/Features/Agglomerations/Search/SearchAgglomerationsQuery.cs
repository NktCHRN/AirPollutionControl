using Application.Dto;
using Application.Queries;
using MediatR;

namespace AccountService.Application.Features.Agglomerations.Search;
public sealed record SearchAgglomerationsQuery(Guid CountryId, int PerPage, int Page, string? SearchText) : PagedSearchQuery(PerPage, Page, SearchText), IRequest<PagedDto<AgglomerationDto>>;
