namespace Application.Queries;
public record PagedSearchQuery(int PerPage, int Page, string? SearchText) : PagedQuery(PerPage, Page);
