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
    public interface IUserManagementService
    {
        LoginRequest IsValidUser(string username, string password);
        LoginRequest LoginDetail();
    }

    public class UserManagementService : IUserManagementService
    {
        private List<LoginRequest> user = new List<LoginRequest>() {
            new LoginRequest() {
                Username = "user@aemenersol.com",
                Password = "Test@123"   
            }
        };

        public LoginRequest IsValidUser(string username , string password)
        {
            if (user.FirstOrDefault(a => a.Username == username && a.Password == password) != null)
                return user.Where(a => a.Username == username && a.Password == a.Password).FirstOrDefault();
            else 
                return null;
        }

        public LoginRequest LoginDetail()
        {
            return user.FirstOrDefault();
        }

    }
}