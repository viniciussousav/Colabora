﻿namespace Colabora.Infrastructure.Services.EmailSender.Models;

public class EmailSettings
{
    public const string ConfigKey = "EmailSettings::Google";
    
    public string SmtpHost { get; set; }

    public int SmtpPort { get; set; }
    
    public string SmtpUser { get; set; }
    
    public string SmtpPass { get; set; }
}