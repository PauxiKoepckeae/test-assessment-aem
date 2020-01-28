using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using TestApiApp.Models;
using TestApiApp.Helpers;

namespace TestApiApp.Helpers
{
   
    public class AppSettings
    {
        // private readonly TokenManagement _tokenManagement;
        private TestApiAppContext _context;
        
        public AppSettings(TestApiAppContext context) 
        {
            _context = context;
            // _tokenManagement = tokenManagement;
        }

        public static string TokenGenerator(string secret, string username)
        {
            var token = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(secret)
            .AddClaim("sub", username)
            .AddClaim("jti", "cf90f8f9-0fbf-4f82-8718-d6598ca47526")
            .AddClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User")
            .AddClaim("exp", 1579970956)
            .AddClaim("iss", "http://testdemo.aem-enersol.com")
            .AddClaim("aud", "http://testdemo.aem-enersol.com")
            .AddClaim("iat", 1579967356).Build();

            // var tokenHandler = new JwtSecurityTokenHandler();
            // var tokenDescriptor = new SecurityTokenDescriptor()
            // {
                // Subject = new ClaimsIdentity(new[] {
                    //  new Claim(JwtRegisteredClaimNames.Sub, ""),
                    // new Claim(JwtRegisteredClaimNames.Jti, ""),                    
                    // new Claim(JwtRegisteredClaimNames.Sub, ""), 
                    // new Claim(JwtRegisteredClaimNames.Iss, "http://testdemo.aem-enersol.com"),
                    // new Claim(JwtRegisteredClaimNames.Aud, "http://testdemo.aem-enersol.com")
                // }),
                // SigningCredentials = credentials
            // };

            // token.Payload["sub"] = "user@aemenersol.com";
            // token.Payload["jti"] = "cf90f8f9-0fbf-4f82-8718-d6598ca47526";
            // token.Payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] = "User";
            // token.Payload["ext"] = 1579970956;
            // token.Payload["iss"] = "http://testdemo.aem-enersol.com";
            // token.Payload["aud"] = "http://testdemo.aem-enersol.com";
            // token.Payload["iat"] = 1579967356;
            
           
            return token;
        }
        
        public static string GeneratorToken()
        {
            // var payload = new Dictionary<string, object>() {
            var payload = new JwtPayload {
                { "sub", "user@aemenersol.com"},
                { "jti", "cf90f8f9-0fbf-4f82-8718-d6598ca47526" },
                { "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User" },
                { "exp", 1579970956 },
                { "iss", "https://localhost:5001" },
                { "aud", "https://localhost:5001" },
                { "iat", 1579967356 }
            };

            var payload2 = new JwtPayload {
                { "username", "user@aemenersol.com" },
                { "password", "Test@123" },
                { "role", "User" },
                { "exp", 1579970956 },
                { "iss", "http://testdemo.aem-enersol.com" },
                { "aud", "http://testdemo.aem-enersol.com" },
                { "iat", 1579967356 }

            };

            byte[] key = Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9");
            var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenHeader = new JwtHeader(credentials);

            var securityToken = new JwtSecurityToken(tokenHeader, payload2);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(securityToken);
            
            // var token = tokenHandler.ReadJwtToken(tokenString);
            
            return tokenString;
        }
        
        public List<Well> GetWell(int id)
        {
            var result = _context.WellSet.Where(a => a.platformId == id).ToList();

            return result;
        }
    
    }
}