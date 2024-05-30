using Core.Exceptions;
using DAL;
using MediatR;

namespace Course.Commands;

public record ToggleCourseNotificationsCommand(int UserId, int CourseUserId) : IRequest;

public class ToggleCourseNotificationsCommandHandler : IRequestHandler<ToggleCourseNotificationsCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ToggleCourseNotificationsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ToggleCourseNotificationsCommand request, CancellationToken ct)
    {
        var (userId, courseUserId) = request;
        
        var studentCourse = await _unitOfWork.StudentCourseRepository.Get(courseUserId, ct);
        if (studentCourse is null || studentCourse.UserId != userId)
        {
            var teacherCourse = await _unitOfWork.TeacherCourseRepository.Get(courseUserId, ct);
            EntityNotFoundException.ThrowIf(teacherCourse is null || teacherCourse.UserId != userId);

            teacherCourse!.IsNotificationsEnabled = !teacherCourse.IsNotificationsEnabled;
            await _unitOfWork.SaveChanges(ct);
            return;
        }
        
        studentCourse.IsNotificationsEnabled = !studentCourse.IsNotificationsEnabled;
        await _unitOfWork.SaveChanges(ct);
    }
}