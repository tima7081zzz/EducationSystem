namespace Core.Exceptions;

public class BaseException : Exception
{
    public static void ThrowIfNull(object? obj)
    {
        throw new BaseException();
    }
}