using Assignment.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Assignment.Jobs;

public class EnqueueAssignmentDeadlineReminderTimerJob : IJob
{
    private readonly IMediator _mediator;
    private readonly ILogger<EnqueueAssignmentDeadlineReminderTimerJob> _logger;

    public EnqueueAssignmentDeadlineReminderTimerJob(IMediator mediator,
        ILogger<EnqueueAssignmentDeadlineReminderTimerJob> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new RunDeadlinesCheckingCommand(), context.CancellationToken);
        
        _logger.LogInformation($"{nameof(EnqueueAssignmentDeadlineReminderTimerJob)} executed");
    }
}