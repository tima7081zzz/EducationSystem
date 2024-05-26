namespace Core.Exceptions;

public class EntityNotFoundException : Exception
{
    public static void ThrowIf(bool condition)
    {
        if (condition)
        {
            throw new EntityNotFoundException();
        }
    }
    
    public static void ThrowIfNull(object? obj)
    {
        if (obj is null)
        {
            throw new EntityNotFoundException();
        }
    }
}