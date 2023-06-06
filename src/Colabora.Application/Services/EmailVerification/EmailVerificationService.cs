using System.Net.Mail;
using Colabora.Application.Commons;
using Colabora.Domain.Shared;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;
using Colabora.Infrastructure.Services.EmailSender;
using Microsoft.Extensions.Logging;
using ErrorMessages = Colabora.Application.Shared.ErrorMessages;

namespace Colabora.Application.Services.EmailVerification;

public class EmailVerificationService : IEmailVerificationService
{
    private readonly ILogger<EmailVerificationService> _logger;
    private readonly IEmailSender _emailSender;
    private readonly IEmailVerificationRepository _emailVerificationRepository;

    public EmailVerificationService(
        ILogger<EmailVerificationService> logger,
        IEmailSender emailSender, 
        IEmailVerificationRepository emailVerificationRepository)
    {
        _logger = logger; 
        _emailSender = emailSender;
        _emailVerificationRepository = emailVerificationRepository;
    }

    public async Task<Result<EmptyResult>> SendEmailVerification(EmailVerificationRequest request)
    {
        try
        {
            if (!IsValidEmail(request.Email))
                return Result.Fail<EmptyResult>(Error.Create("Email", "Invalid email verification request"));

            await _emailVerificationRepository.CreateEmailVerificationRequest(request);
            await _emailSender.SendEmail(request.Email, "Valide seu email", "Test");

            return Result.Success(EmptyResult.Empty);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending email verification at {EmailVerificationService}", nameof(EmailVerificationService));
            return Result.Fail<EmptyResult>(ErrorMessages.CreateInternalError(e.Message));
        }
    }

    private static bool IsValidEmail(string? email) =>
        !string.IsNullOrWhiteSpace(email) || !MailAddress.TryCreate(email!, out _);
}