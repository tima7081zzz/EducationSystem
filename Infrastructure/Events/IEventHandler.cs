using Microsoft.Extensions.Logging;

namespace Events;

public interface IEventHandler
{
    Task<HandlerInvokeResult> Invoke(EventArgs eventArgs, CancellationToken ct);
}

public abstract class BaseEventHandler<TArgs> : IEventHandler where TArgs : EventArgs
{
    protected readonly ILogger<BaseEventHandler<TArgs>> Logger;

    protected BaseEventHandler(ILogger<BaseEventHandler<TArgs>> logger)
    {
        Logger = logger;
    }

    async Task<HandlerInvokeResult> IEventHandler.Invoke(EventArgs eventArgs, CancellationToken ct)
    {
        return await Invoke((TArgs) eventArgs, ct);
    }

    private async Task<HandlerInvokeResult> Invoke(TArgs eventArgs, CancellationToken ct)
    {
        ICollection<KeyValuePair<string, string>>? contextFields = null;

        try
        {
            var result = await InternalInvoke(eventArgs, ct);
            if (result is null)
            {
                throw new NullReferenceException("Result can not be null");
            }

            contextFields = result.ContextFields;

            if (result.Exception is not null)
            {
                Logger.LogError($"Failed to invoke handler {GetType().FullName}", result.Exception);
            }

            return result;
        }
        catch (Exception e)
        {
            Logger.LogError($"Error during invoking handler {GetType().FullName}", e, contextFields);
            return Failed(e, contextFields);
        }
    }

    protected abstract Task<HandlerInvokeResult> InternalInvoke(TArgs eventArgs, CancellationToken ct);

    protected virtual HandlerInvokeResult Failed(Exception exception, ICollection<KeyValuePair<string, string>>? contextFields = null)
    {
        contextFields?.Add(new KeyValuePair<string, string>("Success", false.ToString()));
        var dict = contextFields is not null
            ? new Dictionary<string, string>(contextFields)
            : new Dictionary<string, string>();

        return new HandlerInvokeResult(HandlerInvokeResult.HandlerInvokeResultType.Failed, exception, dict);
    }
    
    protected virtual HandlerInvokeResult Success(ICollection<KeyValuePair<string, string>>? contextFields = null)
    {
        contextFields?.Add(new KeyValuePair<string, string>("Success", true.ToString()));
        var dict = contextFields is not null
            ? new Dictionary<string, string>(contextFields)
            : new Dictionary<string, string>();

        return new HandlerInvokeResult(HandlerInvokeResult.HandlerInvokeResultType.Success, null, dict);
    }
}