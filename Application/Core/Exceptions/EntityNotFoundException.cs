﻿using System.Net;

namespace Core.Exceptions;

public class EntityNotFoundException : HttpNotSuccessException
{
    public EntityNotFoundException(string? message = null) : base(HttpStatusCode.NotFound, message)
    {
    }
    
    public static void ThrowIf(bool condition, string? message = null)
    {
        if (condition)
        {
            throw new EntityNotFoundException(message);
        }
    }
    
    public static void ThrowIfNull(object? obj, string? message = null)
    {
        if (obj is null)
        {
            throw new EntityNotFoundException(message);
        }
    }
}