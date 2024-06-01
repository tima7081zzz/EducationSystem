using Core.Exceptions;
using DAL;
using Emailing;
using Events;
using Events.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Handlers;

[EventBind(typeof(AssignmentGradedEvent))]
public interface IAssignmentGradedEventHandler : IBackgroundEventHandler;

public class AssignmentGradedEventHandler : BaseEventHandler<AssignmentAddedEventArgs>, IAssignmentGradedEventHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IOptionsSnapshot<EmailingOptions> _emailingOptions;

    public AssignmentGradedEventHandler(ILogger<BaseEventHandler<AssignmentAddedEventArgs>> logger,
        IUnitOfWork unitOfWork, IEmailSender emailSender,
        IOptionsSnapshot<EmailingOptions> emailingOptions) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _emailingOptions = emailingOptions;
    }

    protected override async Task<HandlerInvokeResult> InternalInvoke(AssignmentAddedEventArgs eventArgs,
        CancellationToken ct)
    {
        var studentAssignment = await _unitOfWork.StudentAssignmentRepository.Get(eventArgs.AssignmentId, ct);
        EntityNotFoundException.ThrowIfNull(studentAssignment);

        var assignment = await _unitOfWork.AssignmentRepository.Get(studentAssignment!.AssignmentId, ct);
        var studentUser = await _unitOfWork.UserRepository.Get(studentAssignment!.UserId, ct);

        var senderClient = _emailingOptions.Value.NotificationSender;
        await _emailSender.SendEmailAsync(new EmailParams
        {
            FromName = senderClient.Name,
            FromEmail = senderClient.Email,
            Subject = "MyClass. Assignment graded!",
            HtmlBody = $"Hello! Assignment {assignment!.Title} was graded - {studentAssignment.Grade}",
            //TextBody = "Assd",
            ToList = [studentUser!.Email],
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