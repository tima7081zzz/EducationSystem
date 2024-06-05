using DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken ct);
    IDbContextTransaction BeginTransaction();
    UserRepository UserRepository { get; }
    CourseRepository CourseRepository { get; }
    TeacherCourseRepository TeacherCourseRepository { get; }
    StudentCourseRepository StudentCourseRepository { get; }
    AssignmentRepository AssignmentRepository { get; }
    StudentAssignmentAttachmentRepository StudentAssignmentAttachmentRepository { get; }
    StudentAssignmentRepository StudentAssignmentRepository { get; }
    UserNotificationSettingsRepository UserNotificationSettingsRepository { get; }
    RecommendedCourseRepository RecommendedCourseRepository { get; }
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    public UserRepository UserRepository { get; }
    public CourseRepository CourseRepository { get; }
    public TeacherCourseRepository TeacherCourseRepository { get; }
    public StudentCourseRepository StudentCourseRepository { get; }
    public AssignmentRepository AssignmentRepository { get; }
    public StudentAssignmentAttachmentRepository StudentAssignmentAttachmentRepository { get; }
    public StudentAssignmentRepository StudentAssignmentRepository { get; }
    public UserNotificationSettingsRepository UserNotificationSettingsRepository { get; }
    public RecommendedCourseRepository RecommendedCourseRepository { get; }

    public UnitOfWork(DataContext dataContext)
    {
        dataContext.Database.EnsureCreated();
        _dataContext = dataContext;

        UserRepository = new UserRepository(dataContext.Users);
        CourseRepository = new CourseRepository(dataContext.Courses);
        TeacherCourseRepository = new TeacherCourseRepository(dataContext.TeacherCourses);
        StudentCourseRepository = new StudentCourseRepository(dataContext.StudentCourses);
        AssignmentRepository = new AssignmentRepository(dataContext.Assignments);
        StudentAssignmentAttachmentRepository = new StudentAssignmentAttachmentRepository(dataContext.StudentAssignmentAttachments);
        StudentAssignmentRepository = new StudentAssignmentRepository(dataContext.StudentAssignments);
        UserNotificationSettingsRepository = new UserNotificationSettingsRepository(dataContext.UserNotificationSettings);
        RecommendedCourseRepository = new RecommendedCourseRepository(dataContext.RecommendedCourses);
    }
    
    public IDbContextTransaction BeginTransaction()
    {
        return _dataContext.Database.CurrentTransaction ?? _dataContext.Database.BeginTransaction();
    }

    public async Task SaveChanges(CancellationToken ct)
    {
        await _dataContext.SaveChangesAsync(ct);
    }
}