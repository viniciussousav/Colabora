namespace Colabora.Application.Commons;

public partial class Result
{
    public static Result<TValue> Fail<TValue>(Error error) => new(error);
    public static Result<TValue> Success<TValue>(TValue data) => new(data);
}