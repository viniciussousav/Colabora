namespace Colabora.Application.Services.EmailVerification;

public interface IEmailVerificationService
{
    Task SendEmailVerificationRequest(string email, CancellationToken cancellationToken);
}