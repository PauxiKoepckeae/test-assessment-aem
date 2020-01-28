
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TestApiApp.Helpers;
using TestApiApp.Models;

namespace TestApiApp.Services
{
    public interface IAuthenticateService
    {
        LoginRequest IsAuthenticated (string username, string password, out string token);
    }
    
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserManagementService _service;
        private readonly TokenManagement _tokenManagement;
        
        public TokenAuthenticationService(IUserManagementService service, IOptions<TokenManagement> tokenManagement) 
        {
            _service = service;
            _tokenManagement = tokenManagement.Value;
        }

        public LoginRequest IsAuthenticated (string username, string password, out string token)
        {
            token = string.Empty;
            var user = _service.IsValidUser(username, password);
            if (user == null)
            {
                return null;
            }

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                 _tokenManagement.Audience,
                claim,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            user.Password = "";

            return user;
           
        }

    }
}