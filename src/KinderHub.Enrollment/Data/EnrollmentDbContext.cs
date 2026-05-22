using KinderHub.Enrollment.Models;
using Microsoft.EntityFrameworkCore;

namespace KinderHub.Enrollment.Data
{
    public class EnrollmentDbContext : DbContext
    { 
        public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options) : base(options)
        {
            
        }
        public DbSet<Child> Children { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<ClassroomTeacher> ClassroomTeachers { get; set; }
        public DbSet<Waitlist> Waitlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Child>().HasIndex(c => c.ClassroomId);
            modelBuilder.Entity<Child>().HasIndex(c => c.ParentId);

            modelBuilder.Entity<ClassroomTeacher>()
                .HasIndex(ct => new { ct.ClassroomId, ct.TeacherId })
                .IsUnique();

            modelBuilder.Entity<Waitlist>().HasIndex(wl => new { wl.AgeGroup, wl.Status, wl.RequestedDate });

            // Child-Classroom relationship (many to one)
            modelBuilder.Entity<Child>()
                .HasOne(c => c.Classroom)
                .WithMany(cl => cl.Children)
                .HasForeignKey(c => c.ClassroomId)
                .OnDelete(DeleteBehavior.SetNull);

            // ClassroomTeacher - Classroom relationship (many to one)
            modelBuilder.Entity<ClassroomTeacher>()
                .HasOne(ct => ct.Classroom)
                .WithMany(cl => cl.ClassroomTeachers)
                .HasForeignKey(ct => ct.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Waitlist - Child relationship (many to one)
            modelBuilder.Entity<Waitlist>()
                .HasOne(wl => wl.Child)
                .WithMany()
                .HasForeignKey(wl => wl.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Child>(entity => {
            entity.Property(c => c.FirstName).HasMaxLength(100);
            entity.Property(c => c.LastName).HasMaxLength(100);
        });

            modelBuilder.Entity<Classroom>(entity => {
                entity.Property(c => c.Name).HasMaxLength(100);
            });
        }  
    }
}