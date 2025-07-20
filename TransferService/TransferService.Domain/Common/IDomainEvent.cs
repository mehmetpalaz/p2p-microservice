using MediatR;

namespace TransferService.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredAt { get; }
    }
}