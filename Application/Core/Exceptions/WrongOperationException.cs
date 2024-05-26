namespace Core.Exceptions;

public class WrongOperationException : Exception
{
    public static void ThrowIf(bool condition)
    {
        if (condition)
        {
            throw new WrongOperationException();
        }
    }
    
    public static void ThrowIfNull(object? obj)
    {
        if (obj is null)
        {
            throw new WrongOperationException();
        }
    }
}