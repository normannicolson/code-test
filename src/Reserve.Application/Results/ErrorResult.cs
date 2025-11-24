namespace Reserve.Application.Results;

public sealed record ErrorResult<T>(string Code, string Message) : Result<T>;
