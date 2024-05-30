﻿using Core.Exceptions;
using DAL;
using Emailing;
using Events;
using Events.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Handlers;

[EventBind(typeof(AssignmentAddedEvent))]
public interface IAddAssignmentEventHandler : IBackgroundEventHandler;

public class AddAssignmentEventHandler : BaseEventHandler<AssignmentAddedEventArgs>, IAddAssignmentEventHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly IOptionsSnapshot<EmailingOptions> _emailingOptions;
    
    public AddAssignmentEventHandler(ILogger<BaseEventHandler<AssignmentAddedEventArgs>> logger, IUnitOfWork unitOfWork, IEmailSender emailSender, IOptionsSnapshot<EmailingOptions> emailingOptions) : base(logger)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
        _emailingOptions = emailingOptions;
    }

    protected override async Task<HandlerInvokeResult> InternalInvoke(AssignmentAddedEventArgs eventArgs, CancellationToken ct)
    {
        var assignment = await _unitOfWork.AssignmentRepository.Get(eventArgs.AssignmentId, ct);
        EntityNotFoundException.ThrowIfNull(assignment);

        var course = await _unitOfWork.CourseRepository.Get(assignment!.Id, ct);

        var toUsers = await _unitOfWork.StudentCourseRepository.GetUsersForNotification(assignment!.CourseId,
                x => x.NewAssignmentEnabled, ct);
        
        var toEmails = toUsers
            .Select(x => x.Email)
            .ToList();

        var senderClient = _emailingOptions.Value.NotificationSender;
        await _emailSender.SendEmailAsync(new EmailParams
        {
            FromName = senderClient.Name,
            FromEmail = senderClient.Email,
            Subject = "MyClass. New Assignment added!",
            HtmlBody = $"Hello! New Assignment was added to course {course!.Name}",
            //TextBody = "Assd",
            ToList = toEmails,
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