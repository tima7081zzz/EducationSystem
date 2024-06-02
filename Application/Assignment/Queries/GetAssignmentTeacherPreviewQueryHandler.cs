using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Assignment.Queries;

public record GetAssignmentTeacherPreviewQuery(int UserId, int StudentUserId, int AssignmentId)
    : IRequest<AssignmentTeacherPreviewModel>;

public class GetAssignmentTeacherPreviewQueryHandler : IRequestHandler<GetAssignmentTeacherPreviewQuery,
    AssignmentTeacherPreviewModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAssignmentTeacherPreviewQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AssignmentTeacherPreviewModel> Handle(GetAssignmentTeacherPreviewQuery request,
        CancellationToken ct)
    {
        var studentAssignment = await ValidateOperation(request, ct);

        return new AssignmentTeacherPreviewModel
        {
            Id = studentAssignment.Id,
            SubmissionComment = studentAssignment.SubmissionComment,
            StudentAttachments = studentAssignment.StudentAssignmentAttachments.Select(x => new AttachmentModel
            {
                Id = x.Id,
                Name = x.FileName
            }),
        };
    }

    private async Task<StudentAssignment> ValidateOperation(GetAssignmentTeacherPreviewQuery request,
        CancellationToken ct)
    {
        var (userId, studentUserId, assignmentId) = request;

        NotAllowedException.ThrowIf(!await _unitOfWork.AssignmentRepository.IsTeacherForAssignment(userId, assignmentId, ct));

        var studentAssignment = await _unitOfWork.StudentAssignmentRepository.GetWithAttachments(studentUserId, assignmentId, ct);
        EntityNotFoundException.ThrowIfNull(studentAssignment);

        NotAllowedException.ThrowIf(studentAssignment!.Status is StudentCourseTaskStatus.NotSubmitted);

        return studentAssignment;
    }
}

public class AssignmentTeacherPreviewModel
{
    public int Id { get; set; }
    public string? SubmissionComment { get; set; }
    public IEnumerable<AttachmentModel> StudentAttachments { get; set; } = [];
}

