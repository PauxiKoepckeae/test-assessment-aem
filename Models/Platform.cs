using System.Diagnostics.Contracts;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TestApiApp.Models;

namespace TestApiApp.Models
{
    public class Platform
    {
        [Key]
        public int id { get; set; }	
        public string uniqueName { get; set;}
        public double latitude { get; set; }
        public double longitude	{ get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }

        public List<Well> well { get; set; }
    }
}