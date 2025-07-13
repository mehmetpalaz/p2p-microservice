using MediatR;
using TransferService.Application.Interfaces;
using TransferService.Application.Repositories;
using TransferService.Domain.Entities;
using TransferService.Domain.ValueObjects;

namespace TransferService.Application.Commands.CreateTransfer
{
    public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, Guid>
    {

        private readonly ITransferRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTransferCommandHandler(ITransferRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            var transfer = new Transfer(request.SenderUser,
                request.ReceiverUser, 
                new Money(request.Amount, request.Currency));

            await _repository.AddAsync(transfer, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return transfer.Id;
        }
    }
}
