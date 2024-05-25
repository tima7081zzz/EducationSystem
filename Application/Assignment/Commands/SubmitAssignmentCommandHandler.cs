using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Assignment.Commands;

public record SubmitAssignmentCommand(int UserId, int AssignmentId, string? Comment) : IRequest;

public class SubmitAssignmentCommandHandler : IRequestHandler<SubmitAssignmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubmitAssignmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SubmitAssignmentCommand request, CancellationToken ct)
    {
        var (userId, assignmentId, comment) = request;

        var assignment = await ValidateOperation(request, ct);

        var now = DateTimeOffset.UtcNow;
        var status = assignment.Deadline >= now
            ? StudentCourseTaskStatus.SubmittedInTime
            : StudentCourseTaskStatus.SubmittedLate;

        await using var transaction = _unitOfWork.BeginTransaction();
        
        var studentAssignment = await _unitOfWork.StudentAssignmentRepository.Get(userId, assignmentId, ct);
        if (studentAssignment is null)
        {
            studentAssignment = _unitOfWork.StudentAssignmentRepository.Add(new StudentAssignment
            {
                UserId = userId,
                AssignmentId = assignmentId,
            });
            
            await _unitOfWork.SaveChanges(ct);
        }

        studentAssignment.SubmittedAt = now;
        studentAssignment.Status = status;
        studentAssignment.SubmissionComment = comment;

        await _unitOfWork.SaveChanges(ct);
        await transaction.CommitAsync(ct);
    }

    private async Task<DAL.Entities.Assignment> ValidateOperation(SubmitAssignmentCommand request, CancellationToken ct)
    {
        var (userId, assignmentId, _) = request;
        var assignment = await _unitOfWork.AssignmentRepository.Get(assignmentId, ct);
        if (assignment is null)
        {
            throw new EntityNotFoundException();
        }

        var studentCourse = await _unitOfWork.StudentCourseRepository.Get(userId, assignment.CourseId, ct);
        if (studentCourse is null)
        {
            throw new EntityNotFoundException();
        }

        return assignment;
    }
}