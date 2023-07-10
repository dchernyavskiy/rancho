using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.Domain;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BuildingBlocks.Core.Persistence.EfCore.Interceptors;

public class DomainEventCommitterInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventPublisher _domainEventPublisher;

    public DomainEventCommitterInterceptor(IDomainEventPublisher domainEventPublisher)
    {
        _domainEventPublisher = domainEventPublisher;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task UpdateEntities(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<Aggregate<Guid>>())
        {
            var entity = entry.Entity;
            foreach (var @event in entity.GetUncommittedDomainEvents())
            {
                await _domainEventPublisher.PublishAsync(@event);
            }

            entity.MarkUncommittedDomainEventAsCommitted();
        }
    }
}
