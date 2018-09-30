using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Ted.Model;
using Ted.Model.Ads;
using Ted.Model.Auth;
using Ted.Model.Conversations;
using Ted.Model.Network;
using Ted.Model.Notifications;
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
        public DbSet<UserPost> UserAd { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<GlobalString> GlobalStrings { get; set; }
        public DbSet<AdKnn> AdKnns { get; set; }
        public DbSet<SkillKnn> SkillKnns { get; set; }
        public DbSet<PostKnn> PostKnns { get; set; }
        public DbSet<Pknn> PKnns { get; set; }

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
            modelBuilder.Entity<Conversation>().ToTable("Conversations");
            modelBuilder.Entity<Notification>().ToTable("Notifications");
            modelBuilder.Entity<Ad>().ToTable("Ads").HasOne(x => x.Owner);
            modelBuilder.Entity<UserAd>().HasKey(x => new { x.AdId, x.UserId });
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<GlobalString>().ToTable("GlobalStrings");
            modelBuilder.Entity<AdKnn>().ToTable("AdKnns");
            modelBuilder.Entity<SkillKnn>().ToTable("SkillKnns");
            modelBuilder.Entity<PostKnn>().ToTable("PostKnns");
            modelBuilder.Entity<Pknn>().ToTable("PKnns");
        }
    }
}
