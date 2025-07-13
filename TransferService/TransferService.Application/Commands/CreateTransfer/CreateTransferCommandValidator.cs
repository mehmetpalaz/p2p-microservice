using FluentValidation;

namespace TransferService.Application.Commands.CreateTransfer
{
    public class CreateTransferCommandValidator : AbstractValidator<CreateTransferCommand>
    {
        public CreateTransferCommandValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty().Length(3);
            RuleFor(x => x.SenderUser).NotEmpty();
            RuleFor(x => x.ReceiverUser).NotEmpty();
        }
    }
}
