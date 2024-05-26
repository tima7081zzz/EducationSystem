using Core.Exceptions;
using DAL;
using MediatR;

namespace Course.Queries;

public record GetCourseUsersQuery(int UserId, int CourseId) : IRequest<CourseUsersModel>;

public class GetCourseUsersQueryHandler : IRequestHandler<GetCourseUsersQuery, CourseUsersModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseUsersModel> Handle(GetCourseUsersQuery request, CancellationToken ct)
    {
        var studentCourses = await _unitOfWork.StudentCourseRepository.GetByCourse(request.CourseId, ct);
        var teacherCourses = await _unitOfWork.TeacherCourseRepository.GetByCourse(request.CourseId, ct);

        EntityNotFoundException.ThrowIf(teacherCourses.All(x => x.UserId != request.UserId) &&
                                        studentCourses.All(x => x.UserId != request.UserId));

        return new CourseUsersModel
        {
            Teachers = teacherCourses.Select(x => new CourseUserModel {Fullname = x.User.Fullname}),
            Students = studentCourses.Select(x => new CourseUserModel {Fullname = x.User.Fullname}),
        };
    }
}

public class CourseUsersModel
{
    public IEnumerable<CourseUserModel> Teachers { get; set; } = [];
    public IEnumerable<CourseUserModel> Students { get; set; } = [];
}

public class CourseUserModel
{
    public required string Fullname { get; set; }
}