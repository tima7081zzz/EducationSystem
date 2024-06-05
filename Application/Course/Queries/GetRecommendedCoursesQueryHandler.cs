using DAL;
using MediatR;

namespace Course.Queries;

public record GetRecommendedCoursesQuery(int UserId) : IRequest<IEnumerable<UserCourseModel>>;

public class GetRecommendedCoursesQueryHandler : IRequestHandler<GetRecommendedCoursesQuery, IEnumerable<UserCourseModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRecommendedCoursesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserCourseModel>> Handle(GetRecommendedCoursesQuery request, CancellationToken ct)
    {
        var recommendedCourses = (await _unitOfWork.RecommendedCourseRepository.GetForUser(request.UserId, 30, ct))
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

        var courses = recommendedCourses
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return courses;
    }
}