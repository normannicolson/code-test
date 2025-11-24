namespace Reserve.Application.Results;

public sealed record SuccessResult<T>(T Value) : Result<T>;
