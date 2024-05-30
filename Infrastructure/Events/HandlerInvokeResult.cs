namespace Events;

public class HandlerInvokeResult(
    HandlerInvokeResult.HandlerInvokeResultType resultType,
    Exception? exception,
    Dictionary<string, string>? contextFields)
{
    public HandlerInvokeResultType ResultType { get; } = resultType;
    public Exception? Exception { get; } = exception;
    public Dictionary<string, string>? ContextFields { get; } = contextFields;

    public enum HandlerInvokeResultType
    {
        Success = 0,
        Failed = 1,
    }
}