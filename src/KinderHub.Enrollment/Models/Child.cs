using KinderHub.Enrollment.Models.Enums;

namespace KinderHub.Enrollment.Models
{
    public class Child
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public ChildStatus Status { get; set; }
        public Guid? ClassroomId { get; set; }
        public Guid ParentId { get; set; }
        public bool IsPottyTrained { get; set; } = false;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Classroom? Classroom { get; set; }
    }
}