using DAL;
using DAL.Entities;
using MediatR;
using Settings.Queries;

namespace Settings.Commands;

public record UpdateUserSettingsCommand(int UserId, UserNotificationsModel Notifications) : IRequest;

public class UpdateUserSettingsCommandHandler : IRequestHandler<UpdateUserSettingsCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserSettingsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateUserSettingsCommand request, CancellationToken ct)
    {
        var notificationSettings = await _unitOfWork.UserNotificationSettingsRepository.GetByUser(request.UserId, ct) ?? new UserNotificationSettings();

        notificationSettings.IsEnabled = request.Notifications.IsEnabled;
        notificationSettings.NewAssignmentEnabled = request.Notifications.NewAssignmentEnabled;
        notificationSettings.DeadlineReminderEnabled = request.Notifications.DeadlineReminderEnabled;
        notificationSettings.GradingAssignmentEnabled = request.Notifications.GradingAssignmentEnabled;
        notificationSettings.AssignmentSubmittedEnabled = request.Notifications.AssignmentSubmittedEnabled;

        await _unitOfWork.SaveChanges(ct);
    }
}