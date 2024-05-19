namespace Course;

public static class CoursePublicIdGenerator
{
    public static string Generate()
    {
        return Guid.NewGuid().ToString()[..8];
    }
}