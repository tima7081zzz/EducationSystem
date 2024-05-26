using Core.Exceptions;
using DAL;
using DAL.Entities;
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

    public GradeAssignmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
            return;
        }

        await CreateStudentAssignment(request, course, ct);
    }

    private async Task CreateStudentAssignment(GradeAssignmentCommand request, Course course, CancellationToken ct)
    {
        var (userId, studentUserId, assignmentId, grade, gradingComment) = request;
        if (course.StudentCourses.All(x => x.UserId != studentUserId))
        {
            throw new EntityNotFoundException();
        }

        _unitOfWork.StudentAssignmentRepository.Add(new StudentAssignment
        {
            UserId = userId,
            AssignmentId = assignmentId,
            Grade = grade,
            Status = StudentCourseTaskStatus.Graded,
            GradingComment = gradingComment,
        });

        await _unitOfWork.SaveChanges(ct);
    }

    private async Task<Course> ValidateOperation(GradeAssignmentCommand request, CancellationToken ct)
    {
        var (userId, _, assignmentId, grade, _) = request;
        
        var assignment = await _unitOfWork.AssignmentRepository.Get(assignmentId, ct);
        if (assignment is null)
        {
            throw new EntityNotFoundException();
        }

        if (assignment.MaxGrade < grade)
        {
            throw new WrongOperationException();
        }

        var course = await _unitOfWork.CourseRepository.GetWithCourses(assignment.CourseId, ct);
        if (course!.TeacherCourses.All(x=> x.UserId != userId))
        {
            throw new WrongOperationException();
        }

        return course;
    }
}