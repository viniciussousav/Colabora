using System.Net;

namespace Colabora.Application.Commons;

public class Error
{
    private Error() { }
    
    public static readonly Error Empty = new();

    public Error(string code, HttpStatusCode statusCode, string message)
    {
        Code = code;
        Message = message;
        StatusCode = (int) statusCode;
    }

    public Error(string code,  HttpStatusCode statusCode, string message, params object[] args)
    {
        Code = code;
        Message = string.Format(message, args);
        StatusCode = (int) statusCode;
    }
    
    public string Code { get; }
    public string Message { get; }
    public int StatusCode { get; }


}