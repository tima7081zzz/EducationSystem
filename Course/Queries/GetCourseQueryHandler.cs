using Core.Exceptions;
using DAL;
using MediatR;

namespace Course.Queries;

public record GetCourseQuery(int Id, int UserId) : IRequest<CourseModel>;

public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, CourseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseModel> Handle(GetCourseQuery request, CancellationToken ct)
    {
        var course = await _unitOfWork.CourseRepository.GetWithCourses(request.Id, ct);
        var isTeacher = course?.TeacherCourses.Any(x => x.TeacherUserId == request.UserId);
        
        if (course is null || !isTeacher!.Value || course.StudentCourses.All(x => x.UserId != request.UserId))
        {
            throw new EntityNotFoundException();
        }

        return new CourseModel
        {
            Id = course.Id,
            PublicId = course.PublicId,
            Name = course.Name,
            Description = course.Description,
            Category = course.Category,
            IsTeacher = isTeacher.Value
        };
    }
}

public class CourseModel
{
    public int Id { get; set; }
    public required string PublicId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool IsTeacher { get; set; }
}