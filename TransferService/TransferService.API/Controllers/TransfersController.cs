using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransferService.Application.Commands.CreateTransfer;

namespace TransferService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransfersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransfer([FromBody] CreateTransferCommand command)
        {
            var transferId = await _mediator.Send(command);
            
            return Ok(new { TransferId = transferId });
        }
    }
}
