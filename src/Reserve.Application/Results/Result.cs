namespace Reserve.Application.Results;

public abstract record Result<T>
{
    public bool IsSuccess => this is SuccessResult<T>;

    public bool IsFailure => this is ErrorResult<T>;    
}
