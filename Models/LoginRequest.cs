using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TestApiApp.Models
{
    public class LoginRequest
    {
        [Required] 
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [Required] 
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}