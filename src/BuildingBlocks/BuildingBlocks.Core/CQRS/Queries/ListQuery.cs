using BuildingBlocks.Abstractions.CQRS;
using BuildingBlocks.Abstractions.CQRS.Queries;

namespace BuildingBlocks.Core.CQRS.Queries;

public record ListQuery<TResponse> : IListQuery<TResponse>
where TResponse : notnull
{
    public IList<string>? Includes { get; init; }
    public IList<FilterModel>? Filters { get; init; }
    public IList<string>? Sorts { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public record CListQuery<TResponse> : IQuery<TResponse>
where TResponse : notnull
{
    public IList<string>? Includes { get; init; }
    public IList<string> Filters { get; init; } = Enumerable.Empty<string>().ToList();
    public IList<string> Sorts { get; init; } = Enumerable.Empty<string>().ToList();
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
