using DAL;
using Events;
using Events.Events;
using MediatR;

namespace Assignment.Commands;

public record RunDeadlinesCheckingCommand : IRequest;

public class RunDeadlinesCheckingCommandHandler : IRequestHandler<RunDeadlinesCheckingCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRaiser _eventRaiser;

    public RunDeadlinesCheckingCommandHandler(IUnitOfWork unitOfWork, IEventRaiser eventRaiser)
    {
        _unitOfWork = unitOfWork;
        _eventRaiser = eventRaiser;
    }

    public async Task Handle(RunDeadlinesCheckingCommand request, CancellationToken ct)
    {
        var from = DateTimeOffset.Now;
        var to = from.AddMinutes(10);
        
        var assignments = await _unitOfWork.AssignmentRepository.GetAssignmentsToCheckDeadlines(from, to, ct);

        var events = assignments.Select(x =>
            new AssignmentDeadlineApproachingEvent(new AssignmentDeadlineApproachingEventArgs(x.Id)));
        await _eventRaiser.RaiseBatch(events, ct);
    }
}