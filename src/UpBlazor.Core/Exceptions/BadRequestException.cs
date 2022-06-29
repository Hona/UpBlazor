using System;

namespace UpBlazor.Core.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
        
    }
}