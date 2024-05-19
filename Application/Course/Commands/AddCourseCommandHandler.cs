using DAL;
using DAL.Entities;
using MediatR;

namespace Course.Commands;

public record AddCourseCommand(string Name, string? Description, string? Category, int UserId) : IRequest<int>;

public class AddCourseCommandHandler : IRequestHandler<AddCourseCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCourseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AddCourseCommand request, CancellationToken ct)
    {
        var (name, description, category, userId) = request;
        
        var transaction = _unitOfWork.BeginTransaction();
        
        var course = _unitOfWork.CourseRepository.Add(new DAL.Entities.Course
        {
            Name = name,
            Description = description,
            Category = category,
            CreatorUserId = userId,
            CreatedAt = DateTimeOffset.UtcNow,
            PublicId = CoursePublicIdGenerator.Generate()
        });

        _unitOfWork.TeacherCourseRepository.Add(new TeacherCourse
        {
            TeacherUserId = userId,
            CourseId = course.Id
        });

        await _unitOfWork.SaveChanges(ct);
        await transaction.CommitAsync(ct);

        return course.Id;
    }
}