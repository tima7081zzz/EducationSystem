using System.Transactions;
using Core.Exceptions;
using DAL;
using MediatR;

namespace Course.Commands;

public record DeleteCourseCommand(int UserId, int CourseId) : IRequest;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteCourseCommand request, CancellationToken ct)
    {
        var course = await _unitOfWork.CourseRepository.GetWithCourses(request.CourseId, ct);

        EntityNotFoundException.ThrowIfNull(course);
        WrongOperationException.ThrowIf(course!.TeacherCourses.All(x => x.UserId != request.UserId));

        //todo: replace with stored procedure
        
        await using var transaction = _unitOfWork.BeginTransaction();
        
        await _unitOfWork.StudentAssignmentAttachmentRepository.DeleteByCourse(request.CourseId, ct);
        await _unitOfWork.StudentAssignmentRepository.DeleteByCourse(request.CourseId, ct);
        await _unitOfWork.StudentCourseRepository.DeleteByCourse(request.CourseId, ct);
        await _unitOfWork.TeacherCourseRepository.DeleteByCourse(request.CourseId, ct);

        await _unitOfWork.SaveChanges(ct);
        await transaction.CommitAsync(ct);
    }
}