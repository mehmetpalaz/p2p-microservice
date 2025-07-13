using MediatR;

namespace TransferService.Application.Commands.CreateTransfer
{
    public record CreateTransferCommand(Guid SenderUser, Guid ReceiverUser, decimal Amount, string Currency): IRequest<Guid>;
}
