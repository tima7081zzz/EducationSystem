using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Course.Commands;

public record JoinCourseCommand(string PublicId, int UserId) : IRequest;

public class JoinCourseCommandHandler : IRequestHandler<JoinCourseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public JoinCourseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(JoinCourseCommand request, CancellationToken ct)
    {
        var (publicId, userId) = request;

        var course = await _unitOfWork.CourseRepository.GetWithCourses(publicId, ct);
        if (course is null)
        {
            throw new EntityNotFoundException();
        }

        if (course.CreatorUserId == userId || 
            course.TeacherCourses.Any(x => x.TeacherUserId == userId) ||
            course.StudentCourses.Any(x => x.UserId == userId))
        {
            throw new WrongOperationException();
        }

        _unitOfWork.StudentCourseRepository.Add(new StudentCourse
        {
            UserId = userId,
            CourseId = course.Id,
        });

        await _unitOfWork.SaveChanges(ct);
    }
}