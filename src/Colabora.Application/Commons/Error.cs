namespace Colabora.Application.Commons;

public class Error
{
    public static readonly Error Empty = new(string.Empty, string.Empty);
    
    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    private Error(string code, string message, params object[] args)
    {
        Code = code;
        Message = string.Format(message, args);
    }

    public string Code { get; }
    public string Message { get; }
    
    public static Error Create(string code, string message) => new(code, message);
    public static Error Create(string code, string message, params object[] args) => new(code, message, args);
}