using KinderHub.Identity.Models;

namespace KinderHub.Identity.Models
{
    public class TeacherProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateOnly HireDate { get; set; }
        public decimal HourlyRate { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public User User { get; set; } = null!;
    }
}