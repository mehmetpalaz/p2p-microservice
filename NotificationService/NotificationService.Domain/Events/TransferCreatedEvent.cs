namespace NotificationService.Domain.Events
{
    public record TransferCreatedEvent(
      Guid Id,
      Guid SenderUserId,
      Guid ReceiverUserId,
      decimal Amount,
      string Currency,
      DateTime CreatedAt);

}
