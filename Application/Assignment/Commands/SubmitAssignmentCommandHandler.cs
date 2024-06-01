using Core.Exceptions;
using DAL;
using DAL.Entities;
using Events;
using Events.Events;
using MediatR;

namespace Assignment.Commands;

public record SubmitAssignmentCommand(int UserId, int AssignmentId, string? Comment) : IRequest;

public class SubmitAssignmentCommandHandler : IRequestHandler<SubmitAssignmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRaiser _eventRaiser;

    public SubmitAssignmentCommandHandler(IUnitOfWork unitOfWork, IEventRaiser eventRaiser)
    {
        _unitOfWork = unitOfWork;
        _eventRaiser = eventRaiser;
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

        await _eventRaiser.Raise(new AssignmentSubmittedEvent(new AssignmentSubmittedEventArgs(studentAssignment.Id)), ct);
    }

    private async Task<DAL.Entities.Assignment> ValidateOperation(SubmitAssignmentCommand request, CancellationToken ct)
    {
        var (userId, assignmentId, _) = request;
        var assignment = await _unitOfWork.AssignmentRepository.Get(assignmentId, ct);
        EntityNotFoundException.ThrowIfNull(assignment);

        var studentCourse = await _unitOfWork.StudentCourseRepository.Get(userId, assignment!.CourseId, ct);
        EntityNotFoundException.ThrowIfNull(studentCourse);

        return assignment;
    }
}