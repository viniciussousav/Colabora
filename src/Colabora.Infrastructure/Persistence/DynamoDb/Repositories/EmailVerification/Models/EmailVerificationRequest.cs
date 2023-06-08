using System.Text.Json.Serialization;

namespace Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;

public class EmailVerificationRequest
{
    public EmailVerificationRequest(string email)
    {
        Code = Guid.NewGuid();
        Email = email;
        RequestedAt = DateTimeOffset.UtcNow;
    }
    
    public Guid Code { get; init; }

    public string Email { get; init; }
    
    [JsonPropertyName("pk")] 
    public string Pk => Code.ToString();

    [JsonPropertyName("sk")] 
    public string Sk => Code.ToString();
    
    public DateTimeOffset RequestedAt { get; init; }
}