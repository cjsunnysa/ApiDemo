using ApiDemo.Api.Common;
using Microsoft.AspNetCore.Http;
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
        private readonly IRequestHandler<Command, OrderDto> _confirmOrder;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public OrderController(IRequestHandler<Command, OrderDto> confirmOrder, IHttpContextAccessor httpContextAccessor)
        {
            _confirmOrder = confirmOrder;
            _httpContextAccesor = httpContextAccessor;
        }

        [HttpPost]
        [Route("{orderId:int:min(1)}/confirmations")]
        public async Task<IActionResult> Confirm(int orderId, CancellationToken cancellationToken)
        {
            string methodName = "orders/confirmations";
            
            ValidateIdempotencyKey(methodName);

            Command command = new() { OrderId = orderId };

            OrderDto orderDto = await _confirmOrder.Handle(command, cancellationToken);

            SetIdempotentRequestCompleted(methodName);

            return Created($"{_httpContextAccesor.HttpContext.Request.Host.Value}/12345", orderDto);
        }
    }
}
