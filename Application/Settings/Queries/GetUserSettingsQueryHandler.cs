using Core.Exceptions;
using DAL;
using DAL.Entities;
using MediatR;

namespace Settings.Queries;

public record GetUserSettingsQuery(int UserId) : IRequest<UserSettingsModel>;

public class GetUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, UserSettingsModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserSettingsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserSettingsModel> Handle(GetUserSettingsQuery request, CancellationToken ct)
    {
        var userId = request.UserId;
        
        var user = await _unitOfWork.UserRepository.Get(request.UserId, ct);
        EntityNotFoundException.ThrowIfNull(user);

        var notificationSettings = await _unitOfWork.UserNotificationSettingsRepository.GetByUser(userId, ct) ?? new UserNotificationSettings();

        var courseNotifications = await GetCourseNotifications(userId, ct);

        return new UserSettingsModel
        {
            Profile = new UserProfileModel
            {
                Username = user!.Fullname,
                Email = user.Email,
            },
            Notifications = new UserNotificationsModel
            {
                IsEnabled = notificationSettings.IsEnabled,
                NewAssignmentEnabled = notificationSettings.NewAssignmentEnabled,
                DeadlineReminderEnabled = notificationSettings.DeadlineReminderEnabled,
                GradingAssignmentEnabled = notificationSettings.GradingAssignmentEnabled,
                AssignmentSubmittedEnabled = notificationSettings.AssignmentSubmittedEnabled,
            },
            Courses = courseNotifications,
        };
    }

    private async Task<IEnumerable<UserCourseNotificationModel>> GetCourseNotifications(int userId, CancellationToken ct)
    {
        var studentCourses = await _unitOfWork.StudentCourseRepository.GetForUser(userId, ct);
        var teacherCourses = await _unitOfWork.TeacherCourseRepository.GetForUser(userId, ct);

        var courseNotifications = studentCourses
            .Select(x => (new UserCourseNotificationModel
            {
                Id = x.Id,
                Name = x.Course.Name,
                IsEnabled = x.IsNotificationsEnabled,
            }, x.Course.CreatedAt))
            .Concat(teacherCourses.Select(x => (new UserCourseNotificationModel
            {
                Id = x.Id,
                Name = x.Course.Name,
                IsEnabled = x.IsNotificationsEnabled,
            }, x.Course.CreatedAt)))
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.Item1);

        return courseNotifications;
    }
}

public class UserSettingsModel
{
    public required UserProfileModel Profile { get; set; }
    public required UserNotificationsModel Notifications { get; set; }
    public IEnumerable<UserCourseNotificationModel> Courses { get; set; } = [];
}

public class UserProfileModel
{
    public required string Username { get; set; }
    public required string Email { get; set; }
}

public class UserCourseNotificationModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsEnabled { get; set; }
}

public class UserNotificationsModel
{
    public bool IsEnabled { get; set; }
    public bool NewAssignmentEnabled { get; set; }
    public bool DeadlineReminderEnabled { get; set; }
    public bool GradingAssignmentEnabled { get; set; }
    public bool AssignmentSubmittedEnabled { get; set; }
}