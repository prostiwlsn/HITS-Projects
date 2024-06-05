using Auth.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Auth.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        //public AuthDbContext() : base() { }
        //public DbSet<User> Users { get; set; }

        public DbSet<Session> Sessions { get; set; }
    }
}