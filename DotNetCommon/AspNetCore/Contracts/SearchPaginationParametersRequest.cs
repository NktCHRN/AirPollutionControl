namespace AspNetCore.Contracts;
public record SearchPaginationParametersRequest(int PerPage = 10, int Page = 1, string? SearchText = null) : PaginationParametersRequest(PerPage, Page);

