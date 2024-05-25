using DAL;
using MediatR;

namespace Course.Queries;

public record GetUserCoursesQuery(int UserId) : IRequest<IEnumerable<UserCourseModel>>;

public class GetUserCoursesQueryHandler : IRequestHandler<GetUserCoursesQuery, IEnumerable<UserCourseModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserCoursesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserCourseModel>> Handle(GetUserCoursesQuery request, CancellationToken ct)
    {
        var coursesAsTeacher = (await _unitOfWork.TeacherCourseRepository.GetForUser(request.UserId, ct))
            .Select(x => new UserCourseModel
            {
                Id = x.CourseId,
                Name = x.Course.Name,
                Description = x.Course.Description,
                CreatorUserFullName = x.Course.CreatorUser.Fullname,
                IsCreator = x.Course.CreatorUserId == request.UserId,
                CreatedAt = x.Course.CreatedAt,
                IsTeacher = true
            });
        
        var coursesAsStudent = (await _unitOfWork.StudentCourseRepository.GetForUser(request.UserId, ct))
            .Select(x => new UserCourseModel
            {
                Id = x.CourseId,
                Name = x.Course.Name,
                Description = x.Course.Description,
                CreatorUserFullName = x.Course.CreatorUser.Fullname,
                CreatedAt = x.Course.CreatedAt,
                IsCreator = false,
                IsTeacher = false
            });

        var courses = coursesAsStudent
            .Concat(coursesAsTeacher)
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return courses;
    }
}

public class UserCourseModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string CreatorUserFullName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsCreator { get; set; }
    public bool IsTeacher { get; set; }
}