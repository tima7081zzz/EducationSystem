using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Assignment.Commands;

public record SubmitAssignmentCommand(int UserId, int AssignmentId) : IRequest;

public class SubmitAssignmentCommandHandler : IRequestHandler<SubmitAssignmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubmitAssignmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SubmitAssignmentCommand request, CancellationToken ct)
    {
        var (userId, assignmentId) = request;

        var assignment = await ValidateOperation(request, ct);

        var now = DateTimeOffset.UtcNow;
        var status = assignment.Deadline >= now
            ? StudentCourseTaskStatus.SubmittedInTime
            : StudentCourseTaskStatus.SubmittedLate;
        
        _unitOfWork.StudentAssignmentRepository.Add(new StudentAssignment
        {
            UserId = userId,
            AssignmentId = assignmentId,
            SubmittedAt = now,
            Status = status
        });

        await _unitOfWork.SaveChanges(ct);
    }

    private async Task<DAL.Entities.Assignment> ValidateOperation(SubmitAssignmentCommand request, CancellationToken ct)
    {
        var (userId, assignmentId) = request;
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

        var studentAssignment = await _unitOfWork.StudentAssignmentRepository.Get(userId, assignmentId, ct);
        if (studentAssignment is not null)
        {
            throw new WrongOperationException();
        }

        return assignment;
    }
}