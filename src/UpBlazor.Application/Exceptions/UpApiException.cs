using System;
using System.Collections.Generic;
using Up.NET.Models;

namespace UpBlazor.Application.Exceptions;

public class UpApiException : Exception
{
    public IReadOnlyList<ErrorResponse> Errors { get; set; }
    
    public UpApiException() { }
    public UpApiException(List<ErrorResponse> errors)
    {
        Errors = errors.AsReadOnly();
    }
}