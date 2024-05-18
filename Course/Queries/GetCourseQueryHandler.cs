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
        
        if (course is null || (!isTeacher!.Value && course.StudentCourses.All(x => x.UserId != request.UserId)))
        {
            throw new EntityNotFoundException();
        }

        var courseAssignments = await _unitOfWork.AssignmentRepository.GetByCourse(course.Id, ct);

        return new CourseModel
        {
            Id = course.Id,
            PublicId = course.PublicId,
            Name = course.Name,
            Description = course.Description,
            Category = course.Category,
            IsTeacher = isTeacher.Value,
            Assignments = courseAssignments.Select(x => new CourseAssignmentModel
            {
                Id = x.Id,
                Title = x.Title,
                CreatedAt = x.CreatedAt,
            })
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
    public IEnumerable<CourseAssignmentModel> Assignments { get; set; } = [];

}

public class CourseAssignmentModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}