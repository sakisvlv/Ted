using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Ted.Model;
using Ted.Model.Auth;

namespace Ted.Dal
{
    public class Context : IdentityDbContext<User, Role, Guid>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure domain classes using Fluent API here

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Ted");

            //Models
        }
    }
}
