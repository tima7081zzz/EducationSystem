using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Assignment.Queries;

public record GetAssignmentOverviewQuery(int UserId, int Id) : IRequest<GetAssignmentOverviewModel>;

public class GetAssignmentOverviewQueryHandler : IRequestHandler<GetAssignmentOverviewQuery, GetAssignmentOverviewModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAssignmentOverviewQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetAssignmentOverviewModel> Handle(GetAssignmentOverviewQuery request, CancellationToken ct)
    {
        var assignment = await _unitOfWork.AssignmentRepository.Get(request.Id, ct);
        EntityNotFoundException.ThrowIfNull(assignment);

        var studentCourses = await _unitOfWork.StudentCourseRepository.GetByCourse(assignment!.CourseId, ct);

        var studentAssignments = (await _unitOfWork.StudentAssignmentRepository.GetByAssignment(assignment.Id, ct))
            .ToDictionary(x => x.UserId, x => x);

        return GetStatistics(assignment, studentAssignments, studentCourses);
    }

    private static GetAssignmentOverviewModel GetStatistics(DAL.Entities.Assignment assignment,
        IDictionary<int, StudentAssignment> studentAssignments, IEnumerable<StudentCourse> studentCourses)
    {
        return new GetAssignmentOverviewModel
        {
            Id = assignment.Id,
            Title = assignment.Title,
            SubmittedCount = studentAssignments.Values.Count(x => x.SubmittedAt is not null),
            NotSubmittedCount = studentAssignments.Values.Count(x => x.SubmittedAt is not null),
            GradedCount = studentAssignments.Values.Count(x => x.Grade is not null),
            MaxGrade = assignment.MaxGrade,
            StudentAssignmentInfos = studentCourses.Select(x =>
            {
                if (studentAssignments.TryGetValue(x.UserId, out var studentAssignment))
                {
                    return new StudentAssignmentOverviewModel
                    {
                        UserId = x.UserId,
                        UserFullname = x.User.Fullname,
                        Grade = studentAssignment.Grade
                    };
                }

                return new StudentAssignmentOverviewModel
                {
                    UserId = x.UserId,
                    UserFullname = x.User.Fullname,
                    Grade = null
                };
            })
        };
    }
}

public class GetAssignmentOverviewModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int SubmittedCount { get; set; }
    public int NotSubmittedCount { get; set; }
    public int GradedCount { get; set; }
    public int? MaxGrade { get; set; }
    public IEnumerable<StudentAssignmentOverviewModel> StudentAssignmentInfos { get; set; } = [];
}

public class StudentAssignmentOverviewModel
{
    public int UserId { get; set; }
    public required string UserFullname { get; set; }
    public double? Grade { get; set; }
}