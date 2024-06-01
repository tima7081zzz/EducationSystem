using Core.Exceptions;
using DAL;
using Emailing;
using Events;
using Events.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Handlers;

[EventBind(typeof(AssignmentDeadlineApproachingEvent))]
public interface IAssignmentDeadlineApproachingEventHandler : IBackgroundEventHandler;

public class AssignmentDeadlineApproachingEventHandler : BaseEventHandler<AssignmentDeadlineApproachingEventArgs>,
    IAssignmentDeadlineApproachingEventHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IOptionsSnapshot<EmailingOptions> _emailingOptions;

    public AssignmentDeadlineApproachingEventHandler(
        ILogger<BaseEventHandler<AssignmentDeadlineApproachingEventArgs>> logger, IUnitOfWork unitOfWork,
        IEmailSender emailSender, IOptionsSnapshot<EmailingOptions> emailingOptions) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _emailingOptions = emailingOptions;
    }

    protected override async Task<HandlerInvokeResult> InternalInvoke(AssignmentDeadlineApproachingEventArgs eventArgs,
        CancellationToken ct)
    {
        var assignment = await _unitOfWork.AssignmentRepository.Get(eventArgs.AssignmentId, ct);
        EntityNotFoundException.ThrowIfNull(assignment);

        var studentUsers = await _unitOfWork.StudentCourseRepository.GetUsersForNotification(assignment!.CourseId,
                x => x.DeadlineReminderEnabled, ct);
        if (studentUsers.Count == 0)
        {
            return Success();
        }
        
        var senderClient = _emailingOptions.Value.NotificationSender;
        await _emailSender.SendEmailAsync(new EmailParams
        {
            FromName = senderClient.Name,
            FromEmail = senderClient.Email,
            Subject = "MyClass. Assignment deadline reminder!",
            HtmlBody = $"Hello! Assignment {assignment.Title} deadline is approaching! Deadline is {assignment.Deadline}",
            //TextBody = "Assd",
            ToList = studentUsers.Select(x => x.Email).ToList(),
        }, new ClientParams
        {
            Username = senderClient.Username,
            UserPassword = senderClient.UserPassword,
            SmtpHost = senderClient.SmtpHost,
            SmtpPort = senderClient.SmtpPort,
        }, ct);

        return Success();
    }
}