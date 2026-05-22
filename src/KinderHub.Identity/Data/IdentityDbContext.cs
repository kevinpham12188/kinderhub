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

                // Field length constraints
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FirstName).HasMaxLength(100);
                entity.Property(u => u.LastName).HasMaxLength(100);
                entity.Property(u => u.Email).HasMaxLength(255);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
                entity.Property(u => u.PasswordHash).HasMaxLength(60);
            });

            modelBuilder.Entity<ParentProfile>(entity =>
            {
                entity.Property(p => p.HomeAddress).HasMaxLength(300);
                entity.Property(p => p.EmergencyContactName).HasMaxLength(100);
                entity.Property(p => p.EmergencyContactPhone).HasMaxLength(20);
            });

            modelBuilder.Entity<AuthorizedPickup>(entity =>
            {
                entity.Property(a => a.Name).HasMaxLength(100);
                entity.Property(a => a.PhoneNumber).HasMaxLength(20);
            });
        }
  
    }
}