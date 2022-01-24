using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDemo.Api.Common
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message)
            : base(message)
        { }
    }
}
