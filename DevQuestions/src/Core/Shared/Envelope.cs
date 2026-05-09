using System.Text.Json.Serialization;

namespace Shared;

public record Envelope
{
    public object? Result { get; }

    public Failure? FailureList { get; }

    public bool IsFailure => FailureList != null || (FailureList != null & FailureList.Any());

    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    private Envelope(object? result, Failure? failureList)
    {
        Result = result;
        FailureList = failureList;
        TimeGenerated = DateTime.UtcNow;
    }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Failure(Failure failure) =>
        new(null, failure);
}

public record Envelope<T>
{
    public T? Result { get; }

    public Failure? FailureList { get; }

    public bool IsFailure => FailureList != null || (FailureList != null & FailureList.Any());

    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    private Envelope(T? result, Failure? failureList)
    {
        Result = result;
        FailureList = failureList;
        TimeGenerated = DateTime.UtcNow;
    }

    public static Envelope<T> Ok(T? result = default) =>
        new(result, null);

    public static Envelope<T> Failure(Failure failure) =>
        new(default, failure);
}