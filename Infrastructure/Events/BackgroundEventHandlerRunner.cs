using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Events;

public interface IBackgroundEventHandlerRunner
{
    void RunEventHandler(Type eventHandlerType, EventArgs args);
}

public class BackgroundEventHandlerRunner : IBackgroundEventHandlerRunner
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public BackgroundEventHandlerRunner(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void RunEventHandler(Type eventHandlerType, EventArgs args)
    {
        //todo: replace with queue
        Task.Factory.StartNew(async () =>
        {
            using var scope = _serviceScopeFactory.CreateScope();

            try
            {
                var handler = (IEventHandler) scope.ServiceProvider.GetRequiredService(eventHandlerType);
                await handler.Invoke(args, _cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<BackgroundEventHandlerRunner>>();
                logger.LogError(e.InnerException ?? e, $"{nameof(BackgroundEventHandlerRunner)} Failed to run task");
            }
        });
    }
}