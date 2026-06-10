namespace KinderHub.Enrollment.DTOs.Requests
{
    public class UpdateChildRequestDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set;} = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public Guid ParentId { get; set; }
        public bool IsPottyTrained { get; set; }
    }
}