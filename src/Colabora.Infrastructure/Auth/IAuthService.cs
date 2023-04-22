﻿namespace Colabora.Infrastructure.Auth;

public interface IAuthService
{
    Task<AuthResult> Authenticate(AuthProvider authProvider, string token);
}