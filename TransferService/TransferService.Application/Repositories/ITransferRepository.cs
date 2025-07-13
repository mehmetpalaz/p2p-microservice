using TransferService.Domain.Entities;

namespace TransferService.Application.Repositories
{
    public interface ITransferRepository
    {
        Task AddAsync(Transfer transfer, CancellationToken cancellationToken);
    }

}
