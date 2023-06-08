using Colabora.Application.Commons;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;

namespace Colabora.Application.Services.EmailVerification;

public interface IEmailVerificationService
{
    Task<Result<EmptyResult>> SendEmailVerification(EmailVerificationRequest request);
    Task ValidateEmailVerification(Guid verificationCode);
}