namespace AspNetCore.Contracts;
public record PaginationParametersRequest(int PerPage = 10, int Page = 1);
