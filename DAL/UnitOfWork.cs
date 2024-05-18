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
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;

    public IDbContextTransaction BeginTransaction()
    {
        return _dataContext.Database.CurrentTransaction ?? _dataContext.Database.BeginTransaction();
    }

    public UserRepository UserRepository { get; }
    public CourseRepository CourseRepository { get; }
    public TeacherCourseRepository TeacherCourseRepository { get; }
    public StudentCourseRepository StudentCourseRepository { get; }

    public UnitOfWork(DataContext dataContext)
    {
        dataContext.Database.EnsureCreated();
        _dataContext = dataContext;

        UserRepository = new UserRepository(dataContext.Users);
        CourseRepository = new CourseRepository(dataContext.Courses);
        TeacherCourseRepository = new TeacherCourseRepository(dataContext.TeacherCourses);
        StudentCourseRepository = new StudentCourseRepository(dataContext.StudentCourses);
    }

    public async Task SaveChanges(CancellationToken ct)
    {
        await _dataContext.SaveChangesAsync(ct);
    }
}