using MediatR;
using TransferService.Domain.Entities;
using TransferService.Domain.ValueObjects;

namespace TransferService.Application.Commands.CreateTransfer
{
    public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, Guid>
    {
        public async Task<Guid> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            var transfer = new Transfer(request.SenderUser,
                request.ReceiverUser, 
                new Money(request.Amount, request.Currency));

            await Task.CompletedTask;
            
            return transfer.Id;
        }
    }
}
