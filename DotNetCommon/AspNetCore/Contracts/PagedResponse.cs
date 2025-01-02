namespace AspNetCore.Contracts;
public class PagedResponse<TResponse, TPaginationParameters>
{
    public IEnumerable<TResponse> Data { get; set; } = [];
    public TPaginationParameters PaginationParameters { get; set; } = default!;
}
