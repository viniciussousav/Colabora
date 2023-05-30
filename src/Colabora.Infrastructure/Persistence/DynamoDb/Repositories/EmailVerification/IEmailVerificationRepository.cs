using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;

namespace Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification;

public interface IEmailVerificationRepository
{
    Task CreateEmailVerificationRequest(EmailVerificationRequest emailVerificationRequest);
    Task<EmailVerificationRequest?> GetAsync(Guid id);
}