﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Ted.Model;
using Ted.Model.Auth;
using Ted.Model.Network;
using Ted.Model.PersonalSkills;
using Ted.Model.Posts;

namespace Ted.Dal
{
    public class Context : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PersonalSkill> PersonalSkills { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserPost> UserPost { get; set; }
        public DbSet<Friend> Friends { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure domain classes using Fluent API here

            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Ted");
            //Models
            modelBuilder.Entity<Photo>().ToTable("Photos");
            modelBuilder.Entity<PersonalSkill>().ToTable("PersonalSkills");
            modelBuilder.Entity<Experience>().ToTable("Experiences");
            modelBuilder.Entity<Education>().ToTable("Educations");
            modelBuilder.Entity<Post>().ToTable("Posts").HasOne(x => x.Owner);
            modelBuilder.Entity<Comment>().ToTable("Comment").HasOne(x => x.User);
            modelBuilder.Entity<UserPost>().HasKey(x => new { x.PostId, x.UserId });
            modelBuilder.Entity<Friend>().HasKey(x => new { x.FromUserId, x.ToUserId });
            modelBuilder.Entity<Friend>().HasOne(x => x.FromUser).WithMany(x => x.FriendTo).HasForeignKey(x => x.FromUserId).OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Friend>().HasOne(x => x.ToUser).WithMany(x => x.FrendFrom).HasForeignKey(x => x.ToUserId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
