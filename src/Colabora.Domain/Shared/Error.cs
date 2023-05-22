using System.Text.Json.Serialization;

namespace Colabora.Domain.Shared;

[Serializable]
public class Error
{
    public static Error Empty = new Error();

    private Error()
    {
    }

    [JsonConstructor]
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    private Error(string code, string message, params object[] args)
    {
        Code = code;
        Message = string.Format(message, args);
    }

    public string Code { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public static Error Create(string code, string message) => new(code, message);
    public static Error Create(string code, string message, params object[] args) => new(code, message, args);
}