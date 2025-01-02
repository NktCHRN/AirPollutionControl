namespace AspNetCore.Contracts;
public record PagedResponse<TResponse>(IEnumerable<TResponse> Data, int TotalCount);
