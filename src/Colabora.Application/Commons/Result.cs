using Colabora.Domain.Shared.Errors;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Colabora.Application.Commons;

public partial class Result
{
    private readonly int _failureStatusCode;

    protected Result()
    {
    }

    protected Result(Error error, int failureStatusCode)
    {
        Errors = new List<Error> {error};
        FailureStatusCode = failureStatusCode;
    }

    protected Result(IEnumerable<ValidationFailure> validationFailures)
    {
        FailureStatusCode = StatusCodes.Status400BadRequest;
        Errors = validationFailures.Select(f => Error.Create(f.PropertyName, f.ErrorMessage));
    }

    public IEnumerable<Error> Errors { get; } = Enumerable.Empty<Error>();

    public virtual bool IsValid => !Errors.Any();

    public int FailureStatusCode
    {
        get
        {
            if (IsValid || _failureStatusCode == default)
                throw new ResultException("Valid result does not contains failure Status Code");

            return _failureStatusCode;
        }
        private init => _failureStatusCode = value;
    }
}

public class Result<T> : Result
{
    public Result(T? value)
    {
        Value = value;
    }

    public Result(Error error, int failureStatusCode) : base(error, failureStatusCode)
    {
    }

    public Result(IEnumerable<ValidationFailure> validationErrors) : base(validationErrors)
    {
    }

    public T? Value { get; }

    public override bool IsValid => !Errors.Any() && Value is not null;
}

public class EmptyResult
{
    public static readonly EmptyResult Empty = new();
}