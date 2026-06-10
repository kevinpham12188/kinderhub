namespace KinderHub.Enrollment.DTOs.Requests
{
    public class EnrollChildRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set;} = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Guid ClassroomId { get; set; }
        public Guid ParentId { get; set; }
    }
}