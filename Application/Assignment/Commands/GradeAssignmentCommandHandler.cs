using Core.Exceptions;
using DAL;
using DAL.Entities;
using Events;
using Events.Events;
using MediatR;

namespace Assignment.Commands;

public record GradeAssignmentCommand(
    int UserId,
    int StudentUserId,
    int AssignmentId,
    double Grade,
    string? GradingComment) : IRequest;

public class GradeAssignmentCommandHandler : IRequestHandler<GradeAssignmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRaiser _eventRaiser;

    public GradeAssignmentCommandHandler(IUnitOfWork unitOfWork, IEventRaiser eventRaiser)
    {
        _unitOfWork = unitOfWork;
        _eventRaiser = eventRaiser;
    }

    public async Task Handle(GradeAssignmentCommand request, CancellationToken ct)
    {
        var (_, studentUserId, assignmentId, grade, gradingComment) = request;

        var course = await ValidateOperation(request, ct);
        
        var studentAssignment = await _unitOfWork.StudentAssignmentRepository.Get(studentUserId, assignmentId, ct);
        if (studentAssignment is not null)
        {
            studentAssignment.Grade = grade;
            studentAssignment.GradingComment = gradingComment;

            await _unitOfWork.SaveChanges(ct);
            await RaiseEvent(studentAssignment.Id, ct);
            return;
        }

        studentAssignment = await CreateStudentAssignment(request, course, ct);

        await RaiseEvent(studentAssignment.Id, ct);
    }

    private async Task<StudentAssignment> CreateStudentAssignment(GradeAssignmentCommand request, Course course, CancellationToken ct)
    {
        var (userId, studentUserId, assignmentId, grade, gradingComment) = request;
        
        EntityNotFoundException.ThrowIf(course.StudentCourses.All(x => x.UserId != studentUserId));

        var studentAssignment = _unitOfWork.StudentAssignmentRepository.Add(new StudentAssignment
        {
            UserId = userId,
            AssignmentId = assignmentId,
            Grade = grade,
            Status = StudentCourseTaskStatus.Graded,
            GradingComment = gradingComment,
        });

        await _unitOfWork.SaveChanges(ct);

        return studentAssignment;
    }

    private async Task<Course> ValidateOperation(GradeAssignmentCommand request, CancellationToken ct)
    {
        var (userId, _, assignmentId, grade, _) = request;
        
        var assignment = await _unitOfWork.AssignmentRepository.Get(assignmentId, ct);
        EntityNotFoundException.ThrowIfNull(assignment);

        WrongOperationException.ThrowIf(assignment!.MaxGrade < grade);

        var course = await _unitOfWork.CourseRepository.GetWithCourses(assignment.CourseId, ct);
        
        WrongOperationException.ThrowIf(course!.TeacherCourses.All(x=> x.UserId != userId));

        return course;
    }

    private async Task RaiseEvent(int studentAssignmentId, CancellationToken ct)
    {
        await _eventRaiser.Raise(new AssignmentGradedEvent(new AssignmentGradedEventArgs(studentAssignmentId)), ct);
    }
}