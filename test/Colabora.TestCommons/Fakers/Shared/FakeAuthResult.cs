using Bogus;
using Colabora.Infrastructure.Auth;

namespace Colabora.TestCommons.Fakers.Shared;

public static class FakeAuthResult
{
    public static AuthResult Create(string? email = null, bool isValid = true)
    {
        var faker = new Faker();
        return new AuthResult
        {
            Email = email ?? faker.Person.Email,
            Token = faker.Random.String(20, 200),
            Error = isValid ? string.Empty: faker.Random.Words()
        };
    }
}