namespace AspNetCore.Contracts;

public record ApiResponse<T>(T? Data, ErrorResponse? Error)
{
    public bool IsSuccessful => Error is null;
}

public static class ApiResponse
{
    public static ApiResponse<TResponse> FromResult<TResponse>(TResponse data)
    {
        return new ApiResponse<TResponse>(data, null);
    }

    public static ApiResponse<object?> FromError(ErrorResponse error)
    {
        return new ApiResponse<object?>(null, error);
    }
}
