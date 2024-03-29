﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Colabora.Infrastructure.Auth.Exceptions;
using Colabora.Infrastructure.Auth.Google;
using Colabora.Infrastructure.Auth.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Colabora.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IGoogleAuthProvider _googleAuthProvider;
    private readonly JwtSettings _jwtSettings;

    public AuthService(IGoogleAuthProvider googleAuthProvider, IOptions<JwtSettings> securityCredentialOptions)
    {
        _googleAuthProvider = googleAuthProvider;
        _jwtSettings = securityCredentialOptions.Value;
    }

    public async Task<AuthResult> AuthenticateUser(AuthProvider authProvider, string token)
    {
        try
        {
            var userInfo = authProvider switch
            {
                AuthProvider.Google => await _googleAuthProvider.Authenticate(token),
                _ => throw new InvalidAuthProviderException("The given provider is invalid")
            };

            var credentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JwtKey));

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.JwtIssuer,
                audience: _jwtSettings.JwtAudience,
                expires: DateTime.Now.Add(_jwtSettings.ExpirationTime),
                claims: new List<Claim>
                {
                    new(ClaimTypes.Email, userInfo.Email),
                    new(ClaimTypes.Role, Roles.Volunteer)
                },
                signingCredentials: new SigningCredentials(credentials, SecurityAlgorithms.HmacSha256));

            var stringJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResult {Email = userInfo.Email, Token = stringJwt};
        }
        catch (Exception e)
        {
            return new AuthResult {Error = e.Message};
        }
    }
}