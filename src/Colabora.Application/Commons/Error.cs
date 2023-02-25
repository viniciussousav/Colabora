using System.Net;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable CS8618

namespace Colabora.Application.Commons;

public class Error
{
    public Error() { }
    
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

    public string Code { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }


}