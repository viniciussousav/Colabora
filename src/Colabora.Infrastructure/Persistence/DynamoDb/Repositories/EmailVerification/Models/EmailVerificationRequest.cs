using System.Text.Json.Serialization;

namespace Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;

public class EmailVerificationRequest
{
    public Guid Code { get; init; }
    
    public string Email { get; init; }
    
    public DateTimeOffset ExpirationTime { get; init; }

    [JsonPropertyName("pk")] 
    public string Pk => Code.ToString();

    [JsonPropertyName("sk")] 
    public string Sk => Code.ToString();
}