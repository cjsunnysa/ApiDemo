﻿using System;

namespace ApiDemo.Api.Common
{
    public class ConflictException : Exception
    {
        public ConflictException(string message)
            : base(message)
        { }
    }
}
