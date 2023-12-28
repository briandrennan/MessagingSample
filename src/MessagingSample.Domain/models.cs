namespace MessagingSample.Domain;

public record Result(Guid CorrelationId, bool Success, IEnumerable<string>? Errors = null)
{
    public static Result FromSuccess(Guid correlationId)
        => new Result(correlationId, true);

    public static Result<T> FromValue<T>(Guid correlationId, T data)
        => new Result<T>(correlationId, true, data, null);

    public static Result FromError(Guid correlationId, IEnumerable<string> errors)
        => new Result(correlationId, false, errors);

    public static Result<T> FromError<T>(Guid correlationId, IEnumerable<string> errors)
        => new Result<T>(correlationId, false, default, errors);
}

public record Result<T>(Guid CorrelationId, bool Success, T? Data, IEnumerable<string>? Errors = null)
    : Result(CorrelationId, Success, Errors);

public abstract record DomainEvent(Guid CorrelationId);

public abstract record StoreCommand(Guid CorrelationId, string StoreName)
    : DomainEvent(CorrelationId);

public record CreateStoreCommand(Guid CorrelationId, string StoreName)
    : DomainEvent(CorrelationId);

/// <summary>
/// Represents a command to set a value in a store.
/// </summary>
public record SetStoreValueCommand(
    Guid CorrelationId,
    string StoreName,
    string StorageKey,
    string StorageValue)
    : StoreCommand(CorrelationId, StoreName);

public record DeleteStoreCommand(Guid CorrelationId, string StoreName)
    : StoreCommand(CorrelationId, StoreName);
