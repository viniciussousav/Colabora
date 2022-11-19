using System.Net;

namespace Colabora.Application.Commons;

public partial class Result
{
    protected Result() { }

    protected Result(bool skip)
    {
        Skip = skip;
    }

    protected Result(Error error)
    {
        Errors.Add(error);
    }
    
    protected Result(Error error, HttpStatusCode failedStatusCode)
    {
        FailureStatusCode = (int)failedStatusCode;
        Errors.Add(error);
    }
    
    public int FailureStatusCode { get; } = (int)HttpStatusCode.InternalServerError;

    public List<Error> Errors { get; } = new();

    public bool IsValid => !Errors.Any();
    public bool Skip { get; }
}

public class Result<T> : Result
{
    public Result(T? value)
    {
        Value = value;
    }
    
    public Result(T? value, bool skip) : base(skip)
    {
        Value = value;
    }

    public Result(Error error): base(error) {}
    public Result(Error error, HttpStatusCode failedStatusCode) : base(error, failedStatusCode) {}
    
    public T? Value { get; }

}