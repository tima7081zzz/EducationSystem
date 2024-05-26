using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Assignment.Commands;

public record UnsubmitAssignmentCommand(int UserId, int AssignmentId) : IRequest;

public class UnsubmitAssignmentCommandHandler : IRequestHandler<UnsubmitAssignmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnsubmitAssignmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UnsubmitAssignmentCommand request, CancellationToken ct)
    {
        var (userId, assignmentId) = request;

        var studentAssignment = await _unitOfWork.StudentAssignmentRepository.Get(userId, assignmentId, ct);
        EntityNotFoundException.ThrowIfNull(studentAssignment);

        studentAssignment!.SubmittedAt = null;
        studentAssignment.SubmissionComment = null;
        studentAssignment.Status = StudentCourseTaskStatus.NotSubmitted;

        await _unitOfWork.SaveChanges(ct);
    }
}