using Core.Exceptions;
using DAL;
using MediatR;

namespace Assignment.Commands;

public record AddAssignmentCommand(
    int CourseId,
    int? MaxGrade,
    string Title,
    string? Description,
    int UserId,
    DateTimeOffset Deadline) : IRequest<int>;

public class AddAssignmentCommandHandler : IRequestHandler<AddAssignmentCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddAssignmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddAssignmentCommand request, CancellationToken ct)
    {
        var (courseId, maxGrade, title, description, userId, dateTimeOffset) = request;
        
        var course = await _unitOfWork.CourseRepository.GetWithCourses(courseId, ct);
        if (course is null)
        {
            throw new EntityNotFoundException();
        }

        if (course.TeacherCourses.All(x => x.UserId != userId))
        {
            throw new WrongOperationException();
        }
        
        var assignment = _unitOfWork.AssignmentRepository.Add(new DAL.Entities.Assignment
        {
            CourseId = courseId,
            MaxGrade = maxGrade,
            Title = title,
            Description = description,
            CreatorTeacherId = userId,
            Deadline = dateTimeOffset,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await _unitOfWork.SaveChanges(ct);
        
        return assignment.Id;
    }
}