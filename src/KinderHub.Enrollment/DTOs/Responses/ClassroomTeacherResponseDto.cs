namespace KinderHub.Enrollment.DTOs.Responses
{
    public class ClassroomTeacherResponseDto
    {
        public Guid Id { get; set; }
        public Guid ClassroomId { get; set; }
        public Guid TeacherId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}