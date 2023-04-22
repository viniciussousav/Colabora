namespace Colabora.Infrastructure.Auth.Shared;

public class UserAuthInfo
{
    public UserAuthInfo(string email, string name)
    {
        Email = email;
        Name = name;
    }

    public string Email { get; }
    public string Name { get; }
}