namespace BuildingBlocks.Abstractions.CQRS.Commands;

public interface IDeleteCommand<TId, out TResponse> : ICommand<TResponse>
    where TId : struct
    where TResponse : notnull
{
    public TId Id { get; init; }
}

public interface IDeleteCommand<TId> : ICommand
{
    public TId Id { get; init; }
}
