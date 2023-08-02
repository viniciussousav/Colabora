using Colabora.Infrastructure.Persistence.Repositories.EmailVerification.Models;

namespace Colabora.Infrastructure.Persistence.Repositories.EmailVerification;

public interface IEmailVerificationRepository
{
    Task CreateEmailVerificationRequest(EmailVerificationRequest emailVerificationRequest);
    Task<EmailVerificationRequest?> GetAsync(Guid id);
}