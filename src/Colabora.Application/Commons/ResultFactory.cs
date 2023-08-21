using Colabora.Domain.Shared.Errors;
using FluentValidation.Results;

namespace Colabora.Application.Commons;

public partial class Result
{
    public static Result<TValue> Fail<TValue>(Error error, int failureStatusCode = 500) => new(error, failureStatusCode);
    public static Result<TValue> Fail<TValue>(IEnumerable<ValidationFailure> errors) => new(errors);
    public static Result<TValue> Success<TValue>(TValue data) => new(data);
}