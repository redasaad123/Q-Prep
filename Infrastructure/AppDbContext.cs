using Core.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>().ToTable("UsersAccount", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "security");
        }


        public DbSet<AdvancedLevel> AdvancedLevel { get; set; } 
        public DbSet<IntermediateLevel> IntermediateLevel { get; set; } 
        public DbSet<BeginnerLevel> BeginnerLevel { get; set; } 
        public DbSet<Frameworks> Framworks { get; set; } 
        public DbSet<MainTrack> MainTrack { get; set; }
        public DbSet<RequestQuestions> RequestQuestions { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<SaveQuestions> SaveQuestions { get; set; }
        public DbSet<FeedBack> FeedBack { get; set; }
        public DbSet<Recent> Recent { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<Posts> Posts { get; set; }



    }
}
