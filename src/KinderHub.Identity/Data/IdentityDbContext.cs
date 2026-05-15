using KinderHub.Identity.Models;
using KinderHub.Identity.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace KinderHub.Identity.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
            
        } 
        public DbSet<User> Users { get; set; }
        public DbSet<TeacherProfile> TeacherProfiles { get; set;}
        public DbSet<ParentProfile> ParentProfiles { get; set; }
        public DbSet<AuthorizedPickup> AuthorizedPickups { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<TeacherProfile>().HasIndex(u => u.UserId).IsUnique();

            modelBuilder.Entity<ParentProfile>().HasIndex(u => u.UserId).IsUnique();

            // User -> ParentProfile (one-to-one)
            modelBuilder.Entity<User>()
                .HasOne(u => u.ParentProfile) 
                .WithOne(p => p.User)
                .HasForeignKey<ParentProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> TeacherProfile (one-to-one)
            modelBuilder.Entity<User>()
                .HasOne(u => u.TeacherProfile) 
                .WithOne(t => t.User)
                .HasForeignKey<TeacherProfile>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ParentProfile -> AuthorizedPickup (one-to-many)
            modelBuilder.Entity<ParentProfile>()
                .HasMany<AuthorizedPickup>()
                .WithOne(a => a.ParentProfile)
                .HasForeignKey(a => a.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}