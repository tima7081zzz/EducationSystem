using Core.Exceptions;
using DAL;
using Emailing;
using Events;
using Events.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Handlers;

[EventBind(typeof(AssignmentSubmittedEvent))]
public interface IAssignmentSubmittedEventHandler : IBackgroundEventHandler;

public class AssignmentSubmittedEventHandler : BaseEventHandler<AssignmentSubmittedEventArgs>,
    IAssignmentSubmittedEventHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IOptionsSnapshot<EmailingOptions> _emailingOptions;

    public AssignmentSubmittedEventHandler(ILogger<BaseEventHandler<AssignmentSubmittedEventArgs>> logger,
        IUnitOfWork unitOfWork, IEmailSender emailSender,
        IOptionsSnapshot<EmailingOptions> emailingOptions) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _emailingOptions = emailingOptions;
    }

    protected override async Task<HandlerInvokeResult> InternalInvoke(AssignmentSubmittedEventArgs eventArgs,
        CancellationToken ct)
    {
        var studentAssignment = await _unitOfWork.StudentAssignmentRepository.Get(eventArgs.StudentAssignmentId, ct);
        EntityNotFoundException.ThrowIfNull(studentAssignment);

        var assignment = await _unitOfWork.AssignmentRepository.Get(studentAssignment!.AssignmentId, ct);
        var studentUser = await _unitOfWork.UserRepository.Get(studentAssignment.UserId, ct);
        
        var teacherUsers = await _unitOfWork.TeacherCourseRepository.GetUsersForNotification(assignment!.CourseId,
            x => x.User.NotificationSettings.AssignmentSubmittedEnabled, ct);
        
        if (teacherUsers.Count == 0)
        {
            return Success();
        }
        
        var senderClient = _emailingOptions.Value.NotificationSender;
        await _emailSender.SendEmailAsync(new EmailParams
        {
            FromName = senderClient.Name,
            FromEmail = senderClient.Email,
            Subject = "MyClass. Assignment submitted!",
            HtmlBody = $"Hello! Assignment {assignment.Title} was submitted by - {studentUser!.Email}",
            //TextBody = "Assd",
            ToList = teacherUsers.Select(x => x.Email).ToList(),
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