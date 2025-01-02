namespace Application.Dto;
public sealed record PagedDto<TDto>(IReadOnlyCollection<TDto> Data, int TotalCount);
