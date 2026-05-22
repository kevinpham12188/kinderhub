namespace KinderHub.Enrollment.Models
{
    public class ClassroomTeacher
    {
        public Guid Id { get; set; }
        public Guid ClassroomId { get; set; }
        public Guid TeacherId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Classroom Classroom { get; set; } = null!;
    }
}