﻿using System.Text.Json.Serialization;

namespace Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;

public class EmailVerificationRequest
{
    public Guid Code { get; init; }

    public string Email { get; init; } = string.Empty;
    
    [JsonPropertyName("pk")] 
    public string Pk => Code.ToString();

    [JsonPropertyName("sk")] 
    public string Sk => Code.ToString();
}