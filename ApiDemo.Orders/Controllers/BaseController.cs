using ApiDemo.Api.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiDemo.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        private static readonly Dictionary<string, List<Guid>> _completedRequests = new();

        protected void ValidateIdempotencyKey(string methodName)
        {
            if (!HttpContext.Request.Headers.ContainsKey("Idempotency-key"))
            {
                throw new BadRequestException("The request is missing the Idempotency-key header.");
            }

            if (!Guid.TryParse(HttpContext.Request.Headers["Idempotency-key"].ToString(), out var idempotencyKey))
            {
                throw new BadRequestException("The IdempotencyKey header is invalid.");
            }

            if (!_completedRequests.ContainsKey(methodName))
            { 
                _completedRequests.Add(methodName, new List<Guid>());

                return;
            }

            var successKeys = _completedRequests[methodName];

            if (successKeys.Contains(idempotencyKey))
            {
                throw new ConflictException($"The request for Idempotency-key {idempotencyKey} has already been executed successfully.");
            }
        }

        protected void SetIdempotentRequestCompleted(string methodName)
        {
            if (!Guid.TryParse(HttpContext.Request.Headers["Idempotency-key"].ToString(), out var idempotencyKey))
            {
                throw new BadRequestException("The IdempotencyKey header is invalid.");
            }

            if (!_completedRequests.ContainsKey(methodName))
            {
                _completedRequests.Add(methodName, new List<Guid>());
            }

            var successKeys = _completedRequests[methodName];

            if (!successKeys.Contains(idempotencyKey))
            {
                successKeys.Add(idempotencyKey);
            }
        }
    }
}
