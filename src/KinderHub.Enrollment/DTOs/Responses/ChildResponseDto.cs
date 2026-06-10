namespace KinderHub.Enrollment.DTOs.Responses
{
    public class ChildResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set;} = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string AgeGroup { get; set; } = string.Empty;
        public DateOnly EnrollmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid? ClassroomId { get; set; }
        public string? ClassroomName { get; set; } = string.Empty;
        public Guid ParentId { get; set; }
        public bool IsPottyTrained { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}