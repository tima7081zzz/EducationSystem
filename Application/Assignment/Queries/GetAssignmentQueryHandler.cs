using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Assignment.Queries;

public record GetAssignmentQuery(int UserId, int Id) : IRequest<AssignmentModel>;

public class GetAssignmentQueryHandler : IRequestHandler<GetAssignmentQuery, AssignmentModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAssignmentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AssignmentModel> Handle(GetAssignmentQuery request, CancellationToken ct)
    {
        var (userId, id) = request;
        
        var studentAssignment = await _unitOfWork.StudentAssignmentRepository.GetWithAssignmentAndAttachments(userId, id, ct);
        if (studentAssignment is null)
        {
            var assignment = await ValidateOperation(request, ct);
            studentAssignment = new StudentAssignment
            {
                Assignment = assignment,
                AssignmentId = assignment.Id,
                Status = StudentCourseTaskStatus.NotSubmitted,
            };
        }

        return new AssignmentModel
        {
            Id = studentAssignment.Assignment.Id,
            Title = studentAssignment.Assignment.Title,
            Description = studentAssignment.Assignment.Description,
            CreatedAt = studentAssignment.Assignment.CreatedAt,
            Deadline = studentAssignment.Assignment.Deadline,
            MaxGrade = studentAssignment.Assignment.MaxGrade,
            Grade = studentAssignment.Grade,
            Status = studentAssignment.Status,
            SubmissionComment = studentAssignment.SubmissionComment,
            Attachments = studentAssignment.StudentAssignmentAttachments.Select(x=> new AttachmentModel
            {
                Id = x.Id,
                Name = x.FileName,
            })
        };
    }

    private async Task<DAL.Entities.Assignment> ValidateOperation(GetAssignmentQuery request, CancellationToken ct)
    {
        var (userId, id) = request;
        
        var assignment = await _unitOfWork.AssignmentRepository.Get(id, ct);
        EntityNotFoundException.ThrowIfNull(assignment);
        
        var course = await _unitOfWork.CourseRepository.GetWithCourses(assignment!.CourseId, ct);
        var isTeacher = course?.TeacherCourses.Any(x => x.UserId == userId);

        var notHaveAccess = course is null || isTeacher!.Value || course.StudentCourses.All(x => x.UserId != userId);
        EntityNotFoundException.ThrowIf(notHaveAccess);

        return assignment;
    }
}

public class AssignmentModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset Deadline { get; set; }
    public int? MaxGrade { get; set; }
    public double? Grade { get; set; }
    public StudentCourseTaskStatus Status { get; set; }
    public string? SubmissionComment { get; set; }
    public IEnumerable<AttachmentModel> Attachments { get; set; } = [];
}

public class AttachmentModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
}