namespace Colabora.Infrastructure.Auth.Shared;

public class UserAuthInfo
{
    public UserAuthInfo(string error)
    {
        Error = error;
        Email = string.Empty;
        Name = string.Empty;
    }

    public UserAuthInfo(string email, string name)
    {
        Email = email;
        Name = name;
        Error = string.Empty;
    }

    public string Email { get; }
    public string Name { get; }
    public string Error { get; }
}