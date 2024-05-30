using Core.Exceptions;
using DAL;
using MediatR;

namespace Course.Commands;

public record DeleteStudentCommand(int UserId, int StudentCourseId) : IRequest;

public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteStudentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteStudentCommand request, CancellationToken ct)
    {
        var (userId, studentCourseId) = request;
        
        var studentCourse = await _unitOfWork.StudentCourseRepository.Get(studentCourseId, ct);
        EntityNotFoundException.ThrowIfNull(studentCourse);

        var course = await _unitOfWork.CourseRepository.GetWithCourses(studentCourse!.CourseId, ct);
        WrongOperationException.ThrowIf(course!.TeacherCourses.All(x => x.UserId != userId));

        await using var transaction = _unitOfWork.BeginTransaction();
        
        await _unitOfWork.StudentAssignmentAttachmentRepository.DeleteByUser(studentCourse.UserId, ct);
        await _unitOfWork.StudentAssignmentRepository.DeleteByUser(studentCourse.UserId, ct);
        await _unitOfWork.StudentCourseRepository.DeleteByUser(studentCourse.UserId, ct);

        await _unitOfWork.SaveChanges(ct);
        await transaction.CommitAsync(ct);
    }
}