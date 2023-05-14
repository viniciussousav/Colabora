namespace Colabora.Application.Services.EmailVerification.Messages;

public record EmailVerificationRequest
{
    public string Email { get; init; } = string.Empty;
    public DateTimeOffset RequestedAt { get; } = DateTimeOffset.Now;
}