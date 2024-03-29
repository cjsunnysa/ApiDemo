﻿using ApiDemo.Api.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ApiDemo.Api.Controllers
{
    [ApiController]
    public class ErrorController : BaseController
    {
        private static readonly IDictionary<Type, Func<Exception, IActionResult>> _exceptionHandlers = 
            new Dictionary<Type, Func<Exception, IActionResult>>
            {
                { typeof(NotFoundException), CreateResultFromNotFoundException },
                { typeof(ValidationException), CreateResultFromValidationException },
                { typeof(BadRequestException), CreateResultFromBadRequestException },
                { typeof(ConflictException), CreateResultFromConflictException }
            };

        [Route("error-dev")]
        public IActionResult ErrorDevelopment([FromServices] IWebHostEnvironment environment)
        {
            if (environment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("Cannot use this error handler outside of a development environment.");
            }

            return CreateResult(environment);
        }


        [Route("error")]
        public IActionResult Error([FromServices] IWebHostEnvironment environment)
        {
            return CreateResult(environment);
        }
        
        private IActionResult CreateResult(IWebHostEnvironment environment)
        {
            IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return CreateResultFromException(context.Error, environment.EnvironmentName);
        }

        private static IActionResult CreateResultFromException(Exception ex, string environment)
        {
            Type exceptionType = ex.GetType();

            if (_exceptionHandlers.ContainsKey(exceptionType))
            {
                Func<Exception, IActionResult> handler = _exceptionHandlers[exceptionType];

                return handler.Invoke(ex);
            }

            return CreateResultFromUnknownException(ex, environment);
        }

        private static IActionResult CreateResultFromNotFoundException(Exception ex)
        {
            NotFoundException exception = (NotFoundException)ex;

            ProblemDetails problem = new()
            {
                Title = "The specified resource was not found.",
                Detail = exception.Message
            };

            return new NotFoundObjectResult(problem);
        }

        private static IActionResult CreateResultFromValidationException(Exception ex)
        {
            ValidationException exception = (ValidationException)ex;

            ValidationProblemDetails problem = new(exception.Errors);

            return new BadRequestObjectResult(problem);
        }

        private static IActionResult CreateResultFromBadRequestException(Exception ex)
        {
            BadRequestException exception = (BadRequestException)ex;

            ProblemDetails problem = new()
            {
                Title = "The request is invalid.",
                Detail= exception.Message
            };

            return new BadRequestObjectResult(problem);
        }

        private static IActionResult CreateResultFromConflictException(Exception ex)
        {
            ConflictException exception = (ConflictException)ex;

            ProblemDetails problem = new()
            {
                Title = "A conflict has occurred for this request.",
                Detail = exception.Message
            };

            return new ConflictObjectResult(problem);
        }

        private static IActionResult CreateResultFromUnknownException(Exception ex, string environment)
        {
            ProblemDetails problem =
                environment == "Development"
                ? CreateDetailedProblemFromUnkownException(ex)
                : CreateProblemFromUnknownException(ex);

            return new ObjectResult(problem)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        private static ProblemDetails CreateProblemFromUnknownException(Exception _)
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
            };
        }
        
        private static ProblemDetails CreateDetailedProblemFromUnkownException(Exception ex)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            ProblemDetails problem = new()
            {
                Title = ex.Message,
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.StackTrace,
            };

            if (ex.InnerException != null)
            {
                problem.Extensions.Add("additional", CreateDetailedProblemFromUnkownException(ex.InnerException));
            }

            return problem;
        }
    }
}
