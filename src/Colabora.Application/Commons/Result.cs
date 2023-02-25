namespace Colabora.Application.Commons;

public partial class Result
{
    protected Result() { }
    
    protected Result(Error error)
    {
        Error = error;
    }
    
    public Error Error { get; } = Error.Empty;

    public bool IsValid => Error == Error.Empty;

    public int FailureStatusCode => !IsValid
        ? Error.StatusCode
        : throw new ResultException("Valid result does not contain an error status code");
}

public class Result<T> : Result
{
    public Result(T? value)
    {
        Value = value;
    }
    
    public Result(Error error): base(error) {}
    public T? Value { get; }

}