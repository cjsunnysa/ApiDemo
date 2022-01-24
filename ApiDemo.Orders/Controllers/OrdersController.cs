using ApiDemo.Api.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static ApiDemo.Api.Features.ConfirmOrder;

namespace ApiDemo.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IRequestHandler<Command, bool> _confirmOrder;

        public OrderController(IRequestHandler<Command, bool> confirmOrder)
        {
            _confirmOrder = confirmOrder;
        }

        [HttpPatch]
        [Route("{orderId:int:min(1)}/confirm")]
        public async Task<IActionResult> Confirm(int orderId, CancellationToken cancellationToken)
        {
            var methodName = "orders/confirm";
            
            ValidateIdempotencyKey(methodName);
            
            var command = new Command { OrderId = orderId };

            await _confirmOrder.Handle(command, cancellationToken);

            SetIdempotentRequestCompleted(methodName);

            return NoContent();
        }
    }
}
