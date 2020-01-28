using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TestApiApp.Models;

namespace TestApiApp.Models
{
    public class TestApiAppContext: DbContext
    {
        public TestApiAppContext(DbContextOptions<TestApiAppContext> options): base(options) { }

        public DbSet<Platform> PlatformSet { get; set; }
        public DbSet<Well> WellSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Platform>().ToTable("Platform").Property(a => a.id).HasColumnName("id").ValueGeneratedOnAdd();
            modelBuilder.Entity<Platform>().Ignore(a => a.well);

            modelBuilder.Entity<Well>().ToTable("Well").Property(a => a.id).HasColumnName("id").ValueGeneratedOnAdd();
        }

    }
}