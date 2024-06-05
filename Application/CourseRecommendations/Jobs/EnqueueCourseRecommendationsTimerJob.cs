using CourseRecommendations.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CourseRecommendations.Jobs;

public class EnqueueCourseRecommendationsTimerJob : IJob
{
    private readonly IMediator _mediator;
    private readonly ILogger<EnqueueCourseRecommendationsTimerJob> _logger;

    public EnqueueCourseRecommendationsTimerJob(IMediator mediator,
        ILogger<EnqueueCourseRecommendationsTimerJob> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new RunRecommendingCoursesCommand(), context.CancellationToken);

        _logger.LogInformation($"{nameof(EnqueueCourseRecommendationsTimerJob)} executed");
    }
}