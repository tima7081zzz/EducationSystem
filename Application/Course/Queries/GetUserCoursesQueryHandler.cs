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
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Category = x.Category,
                CreatorUserFullName = x.CreatorUserFullName,
                CreatedAt = x.CreatedAt,
                IsCreator = x.CreatorUserId == request.UserId,
                IsTeacher = true
            });
        
        var coursesAsStudent = (await _unitOfWork.StudentCourseRepository.GetForUser(request.UserId, ct))
            .Select(x => new UserCourseModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Category = x.Category,
                CreatorUserFullName = x.CreatorUserFullName,
                CreatedAt = x.CreatedAt,
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
    public string? Category { get; set; }
    public required string CreatorUserFullName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsCreator { get; set; }
    public bool IsTeacher { get; set; }
}